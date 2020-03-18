using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pluspy.Net
{
    public interface ITcpServer : IDisposable
    {
        void Start();
        void Stop();
    }
}
