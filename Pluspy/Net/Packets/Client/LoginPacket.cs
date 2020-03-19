using Pluspy.Core;
using Pluspy.Entities;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Client
{
    public struct LoginPacket : IPacket
    {
        public string Username { get; private set; }
        public string UUID { get; private set; }

        public LoginPacket(UserModel model)
        {
            Username = model.Username;
            UUID = model.UUID;
        }

        public State ReadFrom(MinecraftNetworkStream stream, State state, PacketType type)
        {
            Username = stream.ReadString();
            UUID = stream.ReadString();
            return state;
        }

        public readonly State WriteTo(MinecraftNetworkStream stream, State state, PacketType type)
        {
            stream.WriteString(Username);
            stream.WriteString(UUID);
            return State.Play;
        }
    }
}
