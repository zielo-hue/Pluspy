using Pluspy.Core;
using Pluspy.Enums;

namespace Pluspy.Net
{
    public interface IPacket
    {
        State WriteTo(MinecraftNetworkStream stream, State state, PacketType type);
        State ReadFrom(MinecraftNetworkStream stream, State state, PacketType type);
    }
}
