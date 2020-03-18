using Pluspy.Entities;
using Pluspy.Net;
using Pluspy.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pluspy.Core
{
    public sealed class DefaultMinecraftServer : IMinecraftServer
    {
        private readonly ITcpServer _server;
        private readonly ILogger _logger;
        private readonly ushort _port;

        public string Version { get; } = "20w11a";
        public int ProtocolVersion { get; } = 706;
        public int PlayerCapacity { get; } = 50;
        public bool IsOnline { get; private set; }
        public Text Description { get; set; } = Text.Default;
        public Favicon Icon { get; set; }

        public DefaultMinecraftServer(MinecraftServerConfiguration config)
        {
            _port = config.ServerPort;
            _logger = DefaultLogger.Instance;
            _server = new DefaultTcpServer(new DefaultTcpConnection(_logger), _logger, _port);
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

            _ = Task.Run(_server.Start);
            _logger.LogInformation($"Default server on port {_port}...");
            _logger.LogInformation($"Minecraft Version: {Version}");
            _logger.LogInformation($"Protocol Version: {ProtocolVersion}");
        }

        public void Stop()
        {
            _logger.LogInformation($"Stopping the server...");
            _server.Stop();
            _logger.LogInformation("Server stopped.");
            _server.Dispose();
        }
    }
}
