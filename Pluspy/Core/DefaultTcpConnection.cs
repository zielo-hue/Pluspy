using Pluspy.Entities;
using Pluspy.Net;
using Pluspy.Net.Packets.Client;
using Pluspy.Utilities;
using Pluspy.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Pluspy.Core
{
    public sealed class DefaultTcpConnection : ITcpConnection
    {
        private readonly ILogger _logger;
        private TcpClient? _client;
        private NetworkStream? _stream;
        //private BinaryWriter? _writer;

        public DefaultTcpConnection(ILogger logger)
        {
            _logger = logger;
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

                Dispose();
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
                    "1.16 Snapshot",
                    498, 
                    0,
                    50, 
                    new List<PlayerListSampleEntry> 
                    {
                        new PlayerListSampleEntry("JustNrik", "c41ef456-4ca6-4218-8c94-a20bd17ecc4e")
                    },
                    new Chat 
                    {
                        Text = "idk" 
                    }, null);

                serverListPingResponsePacket.WriteTo(_stream);

                try
                {
                    var latencyPacketLength = _stream.ReadVarInt();
                    var latencyPacketId = _stream.ReadByte();

                    if (latencyPacketId != (int)ClientPacket.ServerListLatency)
                    {
                        _logger.LogInformation($"[Status] Closing socket. Client did not request latency detection.");
                        return;
                    }

                    var playload = _stream.Read<long>();
                    Console.WriteLine(playload);

                    _logger.LogDebug($"Closing socket.");
                }
                catch (EndOfStreamException)
                {
                    _logger.LogDebug("End of Stream.");
                }
                finally
                {
                    Dispose();
                }
            }
        }

        public void Dispose()
        {
            _client?.Close();
            _client?.Dispose();
           // _writer?.Dispose();
            _stream?.Dispose();
        }
    }
}
