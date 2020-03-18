using System;

namespace Pluspy.Net
{
    public interface ITcpServer : IDisposable
    {
        void Start();
        void Stop();
    }
}
