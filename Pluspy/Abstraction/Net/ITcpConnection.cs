using System;
using System.Net.Sockets;

namespace Pluspy.Net
{
    public interface ITcpConnection : IDisposable
    {
        void Handle(TcpClient client);
    }
}
