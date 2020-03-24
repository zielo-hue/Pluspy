using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;
using System;
using System.Runtime.InteropServices;

namespace Pluspy.Net.Packets.Responses
{
    [PacketState(State.Login)]
    public readonly struct LoginSuccessRequestPacket : IRequestPacket
    {
        public byte Id => 0x02;
        public string Username { get; }
        public string UUID { get; }

        public LoginSuccessRequestPacket(UserModel model)
        {
            Username = model.Username;
            UUID = model.UUID;
        }

        public State WriteTo(MinecraftStream stream, State state)
        {
            Guid userGuid = Guid.ParseExact(UUID, "N");
            stream.WriteSpan(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref userGuid, 1)));
            stream.WriteString(Username);
            return State.Play;
        }
    }
}
