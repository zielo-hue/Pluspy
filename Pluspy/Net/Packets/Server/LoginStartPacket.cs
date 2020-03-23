using Pluspy.Attributes;
using Pluspy.Enums;
using Pluspy.Net;

namespace Pluspy.Net.Packets.Server
{
    [Serverbound(State.Login)]
    public struct LoginStartPacket : IPacket
    {
        public string Username { get; private set; }

        public LoginStartPacket(string username)
        {
            Username = username;
        }

        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
        {
            Username = stream.ReadString();
            return state;
        }

        public readonly State WriteTo(MinecraftStream stream, State state, PacketType type)
        {
            stream.WriteString(Username);
            return State.Play;
        }
    }
}
