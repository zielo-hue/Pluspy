using Pluspy.Net;
using Pluspy.Utilities;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pluspy.Core
{
    public sealed class DefaultTcpConnection : ITcpConnection
    {
        private readonly IMinecraftServer _server;
        private readonly ILogger _logger;
        private TcpClient? _client;
        private NetworkStream? _stream;
        private BinaryWriter? _writer;

        public DefaultTcpConnection(IMinecraftServer server, ILogger logger)
        {
            _server = server;
            _logger = logger;
        }

        public async Task HandleAsync(TcpClient client, CancellationToken token = default)
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

                await DisposeAsync();
                return;
            }

            var protocolVersion = _stream.ReadVarInt();
            var serverAddress = _stream.ReadString();
            var serverPort = _stream.Read<ushort>();
            var isRequestingStatus = _stream.ReadVarInt() == 1;

            _logger.LogDebug($"[Handshake] New client connecting with protocol version {protocolVersion}, " +
                $"using server address {serverAddress}:{serverPort}, " +
                $"and {(isRequestingStatus ? "is requesting status information" : "is requesting to login")}.");

            _stream.ReadVarInt();
            _stream.ReadByte();

           // need to add more handling smh
        }

        public void Dispose()
        {
            _client?.Close();
            _client?.Dispose();
            _writer?.Dispose();
            _stream?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            _client?.Close();
            _client?.Dispose();

            if (_writer is object)
                await _writer.DisposeAsync();

            if (_stream is object)
                await _stream.DisposeAsync();
        }
    }
}
