using Pluspy.Entities;
using Pluspy.Net;
using Pluspy.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pluspy.Core
{
    public sealed class DefaultMinecraftServer : IMinecraftServer
    {
        public event EventHandler<IMinecraftServer, MinecraftServerEventArgs>? Log;

        private readonly ITcpServer _server;
        private readonly ILogger _logger;

        public string Version { get; } = "1.16 Snapshot";
        public int ProtocolVersion { get; } = 498;
        public int PlayerCapacity { get; } = 50;
        public bool IsOnline { get; private set; }
        public Chat Description { get; set; } = new Chat
        {
            Color = Color.White,
            Subcomponents = new List<Chat>()
            {
                new Chat
                {
                    Text = "Default Server",
                    IsBold = true,
                    Color = Color.Cyan
                },
                new Chat
                {
                    Text = "Server",
                    Color = Color.White
                }
            }
        };
        public ServerFavicon Icon { get; set; }

        public DefaultMinecraftServer(ITcpServer server)
        {
            _server = server;
            _logger = new DefaultLogger(this);
        }

        public Task StartAsync(CancellationToken token = default)
        {
            if (File.Exists("favicon.png"))
            {
                Icon = ServerFavicon.FromBase64String(Convert.ToBase64String(File.ReadAllBytes("favicon.png")));
                _logger.LogInformation($"Loaded server favicon from file favicon.png");
            }
            else
                _logger.LogWarning($"No favicon found. To enable favicons, save a 64x64 file called \"favicon.png\" into the server's directory.");

            _ = _server.StartAsync(token);
            _logger.LogInformation($"Default server on port 25565. \nMinecraft Version: {Version}\nProtocol Version: {ProtocolVersion}");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken token = default)
        {
            _logger.LogInformation($"Stopping the server...");

            try
            {
                return _server.StopAsync(token);
            }
            finally
            {
                _logger.LogInformation("Server stopped.");
                _server.Dispose();
            }
        }
    }
}
