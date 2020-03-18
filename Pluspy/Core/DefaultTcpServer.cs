using Pluspy.Net;
using System;
using System.Net;
using System.Net.Sockets;

namespace Pluspy.Core
{
    public sealed class DefaultTcpServer : ITcpServer
    {
        private readonly TcpListener _listener;
        private readonly ITcpConnection _connection;
        private readonly ILogger _logger;
        private volatile bool _isDisposed = false;
        private volatile bool _isStopped = false;

        public DefaultTcpServer(ushort port) : this(IPAddress.Any, port)
        {
        }

        public DefaultTcpServer(IPAddress ip, ushort port)
        {
            _connection = new DefaultTcpConnection();
            _logger = DefaultLogger.Instance;
            _listener = new TcpListener(ip, port);
            _listener.Start();
        }

        public void Start()
        {
            while (!_isStopped)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().Name);

                var client = _listener.AcceptTcpClient();
                _logger.LogDebug($"Accepted client: {client.Client.RemoteEndPoint}");
                _connection.Handle(client);
                _logger.LogDebug($"Handled client: {client.Client.RemoteEndPoint}");
            }
        }

        public void Stop()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);

            _isStopped = true;
            _listener.Stop();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            Stop();
            _connection.Dispose();
            _isDisposed = true;
        }
    }
}
