using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Responses
{
    [PacketState(State.Login)]
    public readonly struct LoginSuccessRequestPacket : IRequestPacket
    {
        public byte Id => 0x02;
        public short Length => (short)(Username.Length + UUID.Length);
        public string Username { get; }
        public string UUID { get; }

        public LoginSuccessRequestPacket(UserModel model)
        {
            Username = model.Username;
            UUID = model.UUID;
        }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.WriteString(Username);
            stream.WriteString(UUID); // TODO: https://wiki.vg/Pre-release_protocol#Login_Success
            return State.Play;
        }
    }
}
