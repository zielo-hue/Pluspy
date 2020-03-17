using Pluspy.Net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pluspy.Core
{
    public sealed class DefaultTcpServer : ITcpServer
    {
        private readonly TcpListener _listener;
        private readonly ITcpConnection _connection;
        private bool _isDisposed = false;

        public DefaultTcpServer(ITcpConnection connection)
        {
            if (connection is null)
                throw new ArgumentNullException(nameof(connection));

            _connection = connection;
            _listener = new TcpListener(IPAddress.Any, 25565);
            _listener.Start();
        }

        public async Task StartAsync(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().Name);

                var client = await _listener.AcceptTcpClientAsync();
                _ = _connection.HandleAsync(client, token);
            }
        }

        public Task StopAsync(CancellationToken token = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (token.IsCancellationRequested)
                return Task.FromCanceled(token);

            try
            {
                _listener.Stop();
                return Task.CompletedTask;
            }
            catch (SocketException se)
            {
                return Task.FromException(se);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _connection.Dispose();
            _isDisposed = true;
        }

        public ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return default;

            try
            {
                return _connection.DisposeAsync();
            }
            finally
            {
                _isDisposed = true;
            }
        }
    }
}
