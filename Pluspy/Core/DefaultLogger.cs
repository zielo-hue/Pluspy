using System;

namespace Pluspy.Core
{
    public sealed class DefaultLogger : ILogger
    {
        private readonly IMinecraftServer? _server;

        public DefaultLogger()
        {
        }

        public DefaultLogger(IMinecraftServer server)
        {
            if (server is null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
            _server.Log += OnLog;
        }

        private void OnLog(IMinecraftServer sender, MinecraftServerEventArgs eventArgs)
        {
            if (sender.IsOnline == false)
                _server!.Log -= OnLog;

            Log(eventArgs.Message, eventArgs.LogType);
        }

        public void Log(string message, LogType logType)
        {
            Console.WriteLine("[{0}] {1}", logType, message);
        }
    }
}
