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
