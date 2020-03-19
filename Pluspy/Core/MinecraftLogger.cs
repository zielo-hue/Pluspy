using System;

namespace Pluspy.Core
{
    public sealed class MinecraftLogger : ILogger
    {
        private readonly object _lock = new object();

        public static MinecraftLogger Instance { get; } = new MinecraftLogger();

        private MinecraftLogger()
        {
        }

        public void Log(string message, LogType logType)
        {
            lock (_lock)
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
