namespace Pluspy.Core
{
    public interface ILogger
    {
        void Log(string message, LogType logType);
        void LogInformation(string message)
            => Log(message, LogType.Information);
        void LogWarning(string message)
            => Log(message, LogType.Warning);
        void LogCritical(string message)
            => Log(message, LogType.Critical);
        void LogError(string message)
            => Log(message, LogType.Error);
        void LogDebug(string message)
            => Log(message, LogType.Debug);
    }
}
