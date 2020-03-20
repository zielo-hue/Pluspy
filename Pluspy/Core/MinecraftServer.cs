using Pluspy.Entities;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Pluspy.Core
{
    public sealed class MinecraftServer 
    {
        private readonly MinecraftServerConfiguration _config;
        private readonly TcpListener _listener;
        private readonly MinecraftTcpConnection _connection;
        private readonly MinecraftLogger _logger;
        private volatile bool _isDisposed = false;
        private volatile bool _isStopped = false;

        public string MinecraftVersion { get; } = "20w12a";
        public int ProtocolVersion { get; } = 707;
        public int Capacity { get; } = 50;
        public bool IsOnline { get; private set; }
        public Text Description { get; set; } = Text.Default;
        public Favicon Icon { get; set; }

        public MinecraftServer(MinecraftServerConfiguration config)
        {
            var ip = string.IsNullOrWhiteSpace(config.ServerIp) ? IPAddress.Any : IPAddress.Parse(config.ServerIp);

            _config = config;
            _logger = MinecraftLogger.Instance;
            _connection = new MinecraftTcpConnection(this);
            _logger = MinecraftLogger.Instance;
            _listener = new TcpListener(ip, config.ServerPort);
        }

        public void Start()
        {
            if (File.Exists("favicon.png"))
            {
                Icon = Favicon.FromBase64String(Convert.ToBase64String(File.ReadAllBytes("favicon.png")));
                _logger.LogInformation($"Loaded server favicon from file favicon.png");
            }
            else
                _logger.LogWarning($"No favicon found. To enable favicons, save a 64x64 file called \"favicon.png\" into the server's directory.");

            _ = Task.Run(StartServer);
            _logger.LogInformation($"Default server on port {_config.ServerPort}...");
            _logger.LogInformation($"Minecraft Version: {MinecraftVersion}");
            _logger.LogInformation($"Protocol Version: {ProtocolVersion}");
        }

        private void StartServer()
        {
            _listener.Start();

            while (!_isStopped)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().Name);

                var client = _listener.AcceptTcpClient();
                _logger.LogDebug($"Accepted client: {client.Client.RemoteEndPoint}");
                _connection.Handle(client);
                _logger.LogDebug($"Handled client: {client.Client.RemoteEndPoint}");
            }
        }

        public void Stop()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);

            _logger.LogInformation($"Stopping the server...");
            _listener.Stop();
            _isStopped = true;
            _logger.LogInformation("Server stopped.");
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            Stop();
            _connection.Dispose();
            _isDisposed = true;
        }
    }
}
