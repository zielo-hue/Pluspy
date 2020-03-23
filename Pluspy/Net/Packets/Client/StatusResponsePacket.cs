using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;
using Pluspy.Net;
using System.Text.Json;

namespace Pluspy.Net.Packets.Client
{
    [Clientbound(State.Status)]
    public struct StatusResponsePacket : IPacket
    {
        public ServerModel Model { get; private set; }

        public StatusResponsePacket(ServerModel model)
        {
            Model = model;
        }

        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
        {
            Model = JsonSerializer.Deserialize<ServerModel>(stream.ReadString());
            return state;
        }

        public readonly State WriteTo(MinecraftStream stream, State state, PacketType type)
        {
            stream.WriteString(Model.ToString());
            return state;
        }
    }
}
