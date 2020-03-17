using System;

namespace Pluspy.Core
{
    public sealed class DefaultLogger : ILogger
    {
        public static DefaultLogger Instance { get; } = new DefaultLogger();
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
            if (!sender.IsOnline)
                _server!.Log -= OnLog;

            Log(eventArgs.Message, eventArgs.LogType);
        }

        public void Log(string message, LogType logType)
        {
            lock (this)
            {
                var color = Console.ForegroundColor;

                Console.ForegroundColor = logType switch
                {
                    LogType.Information => ConsoleColor.Cyan,
                    LogType.Warning => ConsoleColor.Yellow,
                    LogType.Critical => ConsoleColor.DarkRed,
                    LogType.Error => ConsoleColor.Red,
                    LogType.Debug => ConsoleColor.Gray,
                    _ => ConsoleColor.White
                };
                Console.WriteLine("[{0}] {1}", logType, message);
                Console.ForegroundColor = color;
            }
        }
    }
}
