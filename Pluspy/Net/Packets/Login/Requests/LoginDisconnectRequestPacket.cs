using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    [PacketState(State.Login)]
    public readonly struct LoginDisconnectRequestPacket : IRequestPacket
    {
        public byte Id => 0x00;
        public short Length => 0;
        public Text Text { get; }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.WriteString(Text.ToString());
            return state;
        }
    }
}
