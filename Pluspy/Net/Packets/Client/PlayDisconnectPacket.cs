using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;
using Pluspy.Net;
using System.Text.Json;

namespace Pluspy.Net.Packets.Client
{
    [Clientbound(State.Play)]
    public struct PlayDisconnectPacket : IPacket
    {
        public Text Text { get; private set; }

        public PlayDisconnectPacket(Text text)
        {
            Text = text;
        }

        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
        {
            Text = JsonSerializer.Deserialize<Text>(stream.ReadString());
            return state;
        }

        public readonly State WriteTo(MinecraftStream stream, State state, PacketType type)
        {
            stream.WriteString(Text.ToString());
            return state;
        }
    }
}
