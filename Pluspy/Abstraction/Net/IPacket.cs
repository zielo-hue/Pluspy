using Pluspy.Enums;

namespace Pluspy.Net
{
    public interface IPacket
    {
        State ReadFrom(MinecraftStream stream, State state, PacketType type);
        State WriteTo(MinecraftStream stream, State state, PacketType type);
    }
}
