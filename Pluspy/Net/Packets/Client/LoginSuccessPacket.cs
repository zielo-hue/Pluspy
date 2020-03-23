using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;
using Pluspy.Net;
using System;
using System.Text;

namespace Pluspy.Net.Packets.Client
{
    [Clientbound(State.Login)]
    public struct LoginSuccessPacket : IPacket
    {
        public string Username { get; private set; }
        public string UUID { get; private set; }

        public LoginSuccessPacket(string username, string uuid)
        {
            Username = username;
            UUID = uuid;
        }

        public LoginSuccessPacket(UserModel model)
        {
            Username = model.Username;
            UUID = model.UUID;
        }

        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
        {
            Username = stream.ReadString();
            UUID = stream.ReadString();
            return state;
        }

        public readonly State WriteTo(MinecraftStream stream, State state, PacketType type)
        {
            stream.WriteString(Username);
            stream.WriteSpan(Encoding.UTF8.GetBytes(UUID).AsSpan());
            return State.Play;
        }
    }
}
