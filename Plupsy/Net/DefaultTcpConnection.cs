using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pluspy.Net
{
    public sealed class DefaultTcpConnection : ITcpConnection
    {
        private bool _disposed = false;
        private readonly TcpListener _listener;
        private readonly CancellationTokenSource _cts;

        public DefaultTcpConnection()
        {
            _cts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Any, 25565);
            _listener.Start();
        }

        public Task HandleAsync(TcpClient client, CancellationToken token = default)
        {
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return default;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _cts.Dispose();
            _disposed = true;
        }
    }
}
