using Pluspy.Attributes;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    [PacketState(State.Login)]
    public struct LoginStartResponsePacket : IResponsePacket
    {
        public byte Id => 0x00;
        public short Length => (short)Username.Length;
        public string Username { get; private set; }

        public LoginStartResponsePacket(string username)
        {
            Username = username;
        }

        public State ReadFrom(MinecraftStream stream, State state)
        {
            Username = stream.ReadString();
            return state;
        }

    }
}
