using Pluspy.Core;
using Pluspy.Entities;
using Pluspy.Enums;
using Pluspy.Net.Packets;
using Pluspy.Net.Packets.Requests;
using Pluspy.Net.Packets.Responses;
using Pluspy.Utilities;
using System;
using System.Buffers;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Pluspy.Net
{
    public sealed class MinecraftConnectionManager
    {
        private readonly MinecraftServer _server;
        private readonly MinecraftLogger _logger;
        private readonly HttpClient _httpClient;
        private TcpClient _client;
        private MinecraftStream _stream;
        private NetworkService _service;

        public MinecraftConnectionManager(MinecraftServer server)
        {
            _httpClient = new HttpClient();
            _logger = MinecraftLogger.Instance;
            _server = server;
        }

        public void Handle(TcpClient client)
        {
            _client = client;
            _stream = new MinecraftStream(new LazyStream(client.GetStream()));
            _service = new NetworkService(_stream);

            var handshake = _service.ReadPacket<HandshakeResponsePacket>();

            _logger.LogDebug(
                $"New client connecting with protocol version {handshake.ProtocolVersion}, " +
                $"using server address {handshake.ServerHostname}:{handshake.ServerPort}, " +
                $"and {(handshake.NextState == State.Status ? "is requesting status information" : "is requesting to login")}.");

            if (handshake.NextState == State.Status)
            {
                var packet = new ServerModel(
                    new ServerVersion(_server.MinecraftVersion, _server.ProtocolVersion),
                    new ServerPlayerList(_server.Capacity, 0, null),
                    Text.Default,
                    default).ToPacket();

                _service.WritePacket(packet);

                try
                {
                    var pingPacket = _service.ReadPacket<StatusPingPacket>();

                    _logger.LogDebug($"Received {pingPacket.Time}");
                    _service.WritePacket(pingPacket);
                    _logger.LogInformation($"Successfully handled Status packet.");
                }
                catch (EndOfStreamException)
                {
                    _logger.LogDebug("End of Stream.");
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error ocurred: {e}");
                }

                Reset();
            }
            else
            {
                var loginRequest = _service.ReadPacket<LoginStartResponsePacket>();
                _logger.LogDebug($" {loginRequest.Username} is attempting to join the game.");

                var rsaProvider = new RSACryptoServiceProvider();
                var verifyTokenRented = MemoryPool<byte>.Shared.Rent(4);
                var verifyToken = verifyTokenRented.Memory[..4];
                verifyToken.Span[3] = 17;
                var publicKey = rsaProvider.ExportSubjectPublicKeyInfo();
                var encryptionRequest = new EncryptionRequestPacket(publicKey, verifyToken);

                _service.WritePacket(encryptionRequest);
                verifyTokenRented.Dispose();

                var encryptionResponse = _service.ReadPacket<EncryptionResponsePacket>();

                encryptionResponse.Decrypt(rsaProvider);
                _logger.LogDebug("Checking verification token...");

                if (verifyToken.Span.SequenceEqual(encryptionResponse.VerificationToken))
                {
                    _logger.LogDebug("Token verified.");

                    Span<byte> inputBytes = stackalloc byte[20 + encryptionResponse.SharedSecret.Length + publicKey.Length];
                    encryptionResponse.SharedSecret.CopyTo(inputBytes.Slice(20));
                    publicKey.CopyTo(inputBytes.Slice(20 + encryptionResponse.SharedSecret.Length));

                    string serverHash = Encryption.SHA1.Digest(inputBytes);
                    string requestUrl = $"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={loginRequest.Username}&serverId={serverHash}";
                    var response = _httpClient.GetAsync(requestUrl).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var user = JsonSerializer.Deserialize<UserModel>(response.Content.ReadAsStringAsync().Result);
                        var userGuid = Guid.ParseExact(user.UUID, "N");

                        var aesTransform = new RijndaelManagedTransformCore(encryptionResponse.SharedSecret, CipherMode.CFB, encryptionResponse.SharedSecret, 128, 8, PaddingMode.None, RijndaelManagedTransformMode.Encrypt);
                        var cryptoStream = new CryptoStream(client.GetStream(), aesTransform, CryptoStreamMode.Write);
                        _stream = new MinecraftStream(new LazyStream(cryptoStream));
                        _service = new NetworkService(_stream);

                        var loginSuccess = new LoginSuccessRequestPacket(user);
                        _service.WritePacket(loginSuccess);

                        _logger.LogDebug($"{user.Username}({userGuid}) has logged in!");
                    }
                    else
                        _logger.LogDebug("Session server request failed.");
                }
                else
                    _logger.LogDebug("Invalid verification token.");
            }
        }

        private void Reset()
        {
            _logger.LogDebug($"[{_client?.Client.RemoteEndPoint}] Diconnecting...");
            _client?.Client.Disconnect(true);
            _logger.LogDebug($"[{_client?.Client.RemoteEndPoint}] Disconnected.");
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
