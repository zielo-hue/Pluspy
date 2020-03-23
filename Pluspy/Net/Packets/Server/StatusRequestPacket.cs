using Pluspy.Attributes;
using Pluspy.Enums;
using Pluspy.Net;

namespace Pluspy.Net.Packets.Server
{
    [Serverbound(State.Status)]
    public readonly struct StatusRequestPacket : IPacket
    {
        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
            => state;

        public State WriteTo(MinecraftStream stream, State state, PacketType type)
            => state;
    }
}
