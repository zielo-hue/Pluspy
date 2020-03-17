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

        public void Start()
        {
            while (true)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().Name);

                var client = _listener.AcceptTcpClient();
                _connection.Handle(client);
            }
        }

        public void Stop()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);

            _listener.Stop();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _connection.Dispose();
            _isDisposed = true;
        }
    }
}
