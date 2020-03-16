using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Plupsy.Net
{
    public interface ITcpConnection : IDisposable, IAsyncDisposable
    {
        Task HandleAsync(TcpClient client, CancellationToken token = default);
    }
}
