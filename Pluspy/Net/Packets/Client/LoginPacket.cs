using Pluspy.Core;
using Pluspy.Entities;
using Pluspy.Enums;
using Pluspy.Utilities;
using System.Net.Sockets;
using System.Text;

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

        public State ReadFrom(NetworkStream stream, State state, PacketType type)
        {
            Username = stream.ReadString();
            UUID = stream.ReadString();
            return state;
        }

        public readonly State WriteTo(NetworkStream stream, State state, PacketType type)
        {
            var writer = new PacketWriter();

            writer.WriteString(Username);
            writer.WriteBytes(Encoding.UTF8.GetBytes(UUID));
            writer.WriteTo(stream);

            return State.Play;
        }
    }
}
