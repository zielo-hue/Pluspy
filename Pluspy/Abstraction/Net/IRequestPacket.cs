using Pluspy.Enums;

namespace Pluspy.Net
{
    public interface IRequestPacket : IPacket
    {
        State WriteTo(MinecraftStream stream, State state);
    }
}
