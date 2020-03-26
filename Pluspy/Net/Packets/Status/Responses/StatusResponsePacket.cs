using Pluspy.Attributes;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Responses
{
    [PacketState(State.Status)]
    public readonly struct StatusResponsesPacket : IResponsePacket
    {
        public byte Id => 0x00;

        public State ReadFrom(MinecraftStream stream, State state)
            => state;
    }
}
