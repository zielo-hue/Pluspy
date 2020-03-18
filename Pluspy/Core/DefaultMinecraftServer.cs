using Pluspy.Entities;
using Pluspy.Net;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Pluspy.Core
{
    public sealed class DefaultMinecraftServer : IMinecraftServer
    {
        private readonly ITcpServer _server;
        private readonly ILogger _logger;
        private readonly MinecraftServerConfiguration _config;

        public string MinecraftVersion { get; } = "20w11a";
        public int ProtocolVersion { get; } = 706;
        public int Capacity { get; } = 50;
        public bool IsOnline { get; private set; }
        public Text Description { get; set; } = Text.Default;
        public Favicon Icon { get; set; }

        public DefaultMinecraftServer(MinecraftServerConfiguration config)
        {
            var ip = string.IsNullOrWhiteSpace(config.ServerIp) ? IPAddress.Any : IPAddress.Parse(config.ServerIp);

            _config = config;
            _logger = DefaultLogger.Instance;
            _server = new DefaultTcpServer(ip, config.ServerPort);
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
            _logger.LogInformation($"Default server on port {_config.ServerPort}...");
            _logger.LogInformation($"Minecraft Version: {MinecraftVersion}");
            _logger.LogInformation($"Protocol Version: {ProtocolVersion}");
        }

        public void Stop()
        {
            _logger.LogInformation($"Stopping the server...");
            _server.Stop();
            _logger.LogInformation("Server stopped.");
            _server.Dispose();
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
