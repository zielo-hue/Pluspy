using System;

namespace Pluspy.Core
{
    public class MinecraftServerEventArgs : EventArgs
    {
        public string Message { get; }
        public LogType LogType { get; }

        public MinecraftServerEventArgs(string message, LogType logType)
        {
            Message = message;
            LogType = LogType;
        }
    }
}
