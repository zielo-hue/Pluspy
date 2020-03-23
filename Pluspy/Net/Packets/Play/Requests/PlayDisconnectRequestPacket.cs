using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;
using System.Text.Json;

namespace Pluspy.Net.Packets.Responses
{
    [PacketState(State.Play)]
    public readonly struct PlayDisconnectRequestPacket : IRequestPacket
    {
        public byte Id => 0x1B;
        public short Length => default;
        public Text Text { get; }

        public PlayDisconnectRequestPacket(Text text)
        {
            Text = text;
        }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.WriteString(Text.ToString());
            return state;
        }
    }
}
