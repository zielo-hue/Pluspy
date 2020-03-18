using Pluspy.Entities;

namespace Pluspy.Core
{
    public interface IMinecraftServer
    {
        string Version { get; }
        int ProtocolVersion { get; }
        int PlayerCapacity { get; }
        bool IsOnline { get; }
        Text Description { get; set; }
        Favicon Icon { get; set; }
        void Start();
        void Stop();
    }
}
