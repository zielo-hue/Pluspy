using Pluspy.Entities;
using Pluspy.Net;

namespace Pluspy.Core
{
    public interface IMinecraftServer : ITcpServer
    {
        bool IsOnline { get; }
        string MinecraftVersion { get; }
        int ProtocolVersion { get; }
        int Capacity { get; }
        Text Description { get; set; }
        Favicon Icon { get; set; }
    }
}
