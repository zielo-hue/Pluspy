using Pluspy.Enums;

namespace Pluspy.Net
{
    public interface IResponsePacket : IPacket
    {
        State ReadFrom(MinecraftStream stream, State state);
    }
}
