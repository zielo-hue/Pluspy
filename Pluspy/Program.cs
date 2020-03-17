using Pluspy.Core;
using System;
using System.Threading.Tasks;

namespace Pluspy
{
    class Program
    {
        static async Task Main()
        {
            var connection = new DefaultTcpConnection(new DefaultLogger());
            var tcpServer = new DefaultTcpServer(connection);
            var server = new DefaultMinecraftServer(tcpServer);

            server.Start();
            await Task.Delay(-1);
        }
    }
}
