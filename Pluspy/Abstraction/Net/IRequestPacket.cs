using Pluspy.Enums;

namespace Pluspy.Net
{
    public interface IRequestPacket : IPacket
    {
        short Length { get; }
        State WriteTo(MinecraftStream stream, State state);
    }
}
