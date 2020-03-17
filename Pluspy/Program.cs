using Pluspy.Core;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pluspy
{
    class Program
    {
        static async Task Main()
        {
            var config = File.ReadAllLines("server.properties")
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.TrimStart()[0] != '#')
                .Select(x => x.Split('='))
                .ToDictionary(x => x[0].Trim(), x => x[1].Trim());
            var connection = new DefaultTcpConnection(new DefaultLogger());
            var tcpServer = new DefaultTcpServer(connection, ushort.Parse(config["server-port"]));
            var server = new DefaultMinecraftServer(tcpServer);

            server.Start();
            await Task.Delay(-1);
        }
    }
}
