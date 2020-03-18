﻿using Pluspy.Entities;
using Pluspy.Net;
using Pluspy.Net.Packets.Client;
using Pluspy.Constants;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;

namespace Pluspy.Core
{
    public sealed class DefaultTcpConnection : ITcpConnection
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private TcpClient? _client;
        private NetworkStream? _stream;

        public DefaultTcpConnection()
        {
            _logger = DefaultLogger.Instance;
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
                var serverListPingResponsePacket = new ServerListPingResponsePacket(
                    "10w11a",
                    protocolVersion, 
                    0,
                    50, 
                    new List<UserModel> 
                    {
                        new UserModel("JustNrik", "c41ef456-4ca6-4218-8c94-a20bd17ecc4e")
                    },
                    new Text 
                    {
                        Content = "idk" 
                    }, 
                    null);

                serverListPingResponsePacket.WriteTo(_stream);

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

                _logger.LogDebug($"The user {username} is attempting to join the game.");
                _logger.LogDebug($"Retrieving UUID from mojang api...");

                var response = _httpClient.GetStringAsync($"https://api.mojang.com/users/profiles/minecraft/{username}").Result;

                _logger.LogDebug($"Response: {response}");
                JsonSerializer.Deserialize<UserModel>(response).ToPacket().WriteTo(_stream);
                _logger.LogDebug($"The user {username} logged in!");
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
