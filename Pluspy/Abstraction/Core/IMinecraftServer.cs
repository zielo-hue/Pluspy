using Pluspy.Entities;

namespace Pluspy.Core
{
    public interface IMinecraftServer
    {
        event EventHandler<IMinecraftServer, MinecraftServerEventArgs>? Log;
        string Version { get; }
        int ProtocolVersion { get; }
        int PlayerCapacity { get; }
        public bool IsOnline { get; }
        Chat Description { get; set; }
        ServerFavicon Icon { get; set; }
        void Start();
        void Stop();
    }
}
