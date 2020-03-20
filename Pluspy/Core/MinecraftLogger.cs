using System;

namespace Pluspy.Core
{
    public sealed class MinecraftLogger
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

        public void LogInformation(string message)
            => Log(message, LogType.Information);
        public void LogWarning(string message)
            => Log(message, LogType.Warning);
        public void LogCritical(string message)
            => Log(message, LogType.Critical);
        public void LogError(string message)
            => Log(message, LogType.Error);
        public void LogDebug(string message)
            => Log(message, LogType.Debug);
    }
}
