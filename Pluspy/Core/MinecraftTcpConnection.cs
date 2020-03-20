﻿using Pluspy.Entities;
using Pluspy.Enums;
using Pluspy.Net.Packets;
using Pluspy.Net.Packets.Client;
using Pluspy.Net.Packets.Server;
using Pluspy.Utilities;
using System;
using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.Json;

namespace Pluspy.Core
{
    public sealed class MinecraftTcpConnection
    {
        private readonly MinecraftLogger _logger;
        private readonly HttpClient _httpClient;
        private TcpClient _client;
        private NetworkStream _stream;

        public MinecraftTcpConnection()
        {
            _logger = MinecraftLogger.Instance;
            _httpClient = new HttpClient();
        }

        public void Handle(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            _stream.ReadVarInt();

            var packetId = _stream.ReadByte();

            if (packetId != 0)
            {
                if (packetId == 0xFE)
                    _logger.LogError("Received 0xFE Legacy Ping packet, the server must have sent the client incorrect data.");
                else
                    _logger.LogError($"Received unknown packet with ID 0x{packetId:x2}.");

                Reset();
                return;
            }

            var protocolVersion = _stream.ReadVarInt();
            var serverAddress = _stream.ReadString();
            var serverPort = _stream.Read<ushort>();
            var isRequestingStatus = _stream.ReadVarInt() == 1;

            _logger.LogDebug(
                $"New client connecting with protocol version {protocolVersion}, " +
                $"using server address {serverAddress}:{serverPort}, " +
                $"and {(isRequestingStatus ? "is requesting status information" : "is requesting to login")}.");

            _stream.ReadVarInt();
            _stream.ReadByte();

            if (isRequestingStatus)
            {
                new ServerListPingResponseModel(
                    new ServerListPingResponseVersion("20w11a", protocolVersion),
                    new ServerListPingResponsePlayerList(50, 0, null),
                    Text.Default,
                    default).ToPacket().WriteTo(_stream, default, default);

                try
                {
                    _stream.ReadVarInt();
                    var latencyPacketId = _stream.ReadByte();

                    if (latencyPacketId != (int)ClientPacket.ServerListLatency)
                    {
                        _logger.LogInformation($"Closing socket. Client did not request latency detection.");
                        return;
                    }

                    _logger.LogDebug($"Received ClientPacket: {ClientPacket.ServerListLatency}");

                    var playload = _stream.Read<long>();

                    _logger.LogDebug($"Received {playload}");
                }
                catch (EndOfStreamException)
                {
                    _logger.LogDebug("End of Stream.");
                }
                finally
                {
                    Reset();
                }
            }
            else
            {
                var username = _stream.ReadString();
                _logger.LogDebug($"{username} is attempting to join the game.");

                var rsaProvider = new RSACryptoServiceProvider();
                var verifyToken = MemoryPool<byte>.Shared.Rent(4).Memory;
                var publicKey = rsaProvider.ExportSubjectPublicKeyInfo().AsMemory();
                var encryptionRequest = new EncryptionRequest(publicKey, verifyToken);
                encryptionRequest.WriteTo(_stream, default, default);

                var length = _stream.ReadVarInt();
                var id = _stream.ReadVarInt();

                var encryptionResponse = new EncryptionResponse(_stream);
                encryptionResponse.Decrypt(rsaProvider);

                _logger.LogDebug("Checking verification token...");
                if (verifyToken.Span.SequenceEqual(encryptionResponse.VerifyToken))
                {
                    _logger.LogDebug("Token verified.");
                    Span<byte> inputBytes = stackalloc byte[20 + encryptionResponse.SharedSecret.Length + publicKey.Length];
                    encryptionResponse.SharedSecret.CopyTo(inputBytes.Slice(20));
                    publicKey.Span.CopyTo(inputBytes[(20 + encryptionResponse.SharedSecret.Length)..]);

                    string serverHash = Encryption.SHA1.Digest(inputBytes);

                    string requestUrl = $"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={username}&serverId={serverHash}";
                    var response = _httpClient.GetAsync(requestUrl).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var user = JsonSerializer.Deserialize<UserModel>(response.Content.ReadAsStringAsync().Result);
                        string formattedUuid = Guid.ParseExact(user.UUID, "N").ToString();

                        var aesTransform = new RijndaelManagedTransformCore(encryptionResponse.SharedSecret, CipherMode.CFB, encryptionResponse.SharedSecret, 128, 8, PaddingMode.None, RijndaelManagedTransformMode.Encrypt);
                        var cipherStream = new CryptoStream(_stream, aesTransform, CryptoStreamMode.Write);

                        Span<byte> loginSuccessSpan = stackalloc byte[1024];
                        PacketWriter writer = new PacketWriter(loginSuccessSpan, 0x02);
                        writer.WriteString(formattedUuid);
                        writer.WriteString(user.Username);
                        writer.WriteTo(cipherStream);

                        _logger.LogDebug($"{user.Username}({formattedUuid}) has logged in!");
                    }
                    else
                    {
                        _logger.LogDebug("Session server request failed.");
                    }
                }
                else
                    _logger.LogDebug("Invalid verification token.");
            }
        }

        private void Reset()
        {
            _logger.LogDebug($"Diconnecting the current client");
            _client?.Client.Disconnect(true);
            _logger.LogDebug("Disconnected. Waiting for new clients...");
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
