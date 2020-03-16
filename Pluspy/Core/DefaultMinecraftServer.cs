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
    public sealed class DefaultMinecraftServer : IMinecraftServer, ILogger
    {
        public const string Version = "1.16 Snapshot";
        public const int ProtocolVersion = 498;

        public event EventHandler<IMinecraftServer, MinecraftServerEventArgs>? Log;

        private readonly ITcpServer _server;

        public int PlayerCapacity { get; } = 50;
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
        }

        public Task StartAsync(CancellationToken token)
        {
            if (File.Exists("favicon.png"))
            {
                Icon = ServerFavicon.FromBase64String(Convert.ToBase64String(File.ReadAllBytes("favicon.png")));
                ((ILogger)this).LogInformation($"Loaded server favicon from file favicon.png");
            }
            else
                ((ILogger)this).LogWarning($"No favicon found. To enable favicons, save a 64x64 file called \"favicon.png\" into the server's directory.");

            _ = _server.StartAsync(token);
            ((ILogger)this).LogInformation($"Default server on port 25565. \nMinecraft Version: {Version}\nProtocol Version: {ProtocolVersion}");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken token)
        {
            ((ILogger)this).LogInformation($"Stopping the server...");

            try
            {
                return _server.StopAsync(token);
            }
            finally
            {
                ((ILogger)this).LogInformation("Server stopped.");
                _server.Dispose();
            }
        }

        void ILogger.Log(string message, LogType logType)
            => Log?.Invoke(this, new MinecraftServerEventArgs(message, logType));
    }
}
