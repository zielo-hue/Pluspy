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
            if (!File.Exists("server.properties"))
                File.AppendAllText("server.properties", ConfigSample);

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

        private const string ConfigSample = @"#Minecraft server properties
#Mon Feb 24 19:11:21 GMT 2020
spawn-protection=16
max-tick-time=60000
query.port=25565
generator-settings=
force-gamemode=false
allow-nether=true
enforce-whitelist=false
gamemode=survival
broadcast-console-to-ops=true
enable-query=false
player-idle-timeout=0
difficulty=easy
spawn-monsters=true
broadcast-rcon-to-ops=true
op-permission-level=4
pvp=true
snooper-enabled=true
level-type=default
hardcore=false
enable-command-block=false
max-players=20
network-compression-threshold=256
resource-pack-sha1=
max-world-size=29999984
function-permission-level=2
rcon.port=25575
server-port=25565
server-ip=
spawn-npcs=true
allow-flight=true
level-name=meowpuffygottem
view-distance=10
resource-pack=
spawn-animals=true
white-list=false
rcon.password=
generate-structures=true
max-build-height=256
online-mode=true
level-seed=
prevent-proxy-connections=false
use-native-transport=true
enable-rcon=false
motd=moincraft";
    }
}
