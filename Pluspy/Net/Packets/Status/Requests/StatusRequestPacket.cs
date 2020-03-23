using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    [PacketState(State.Status)]
    public readonly struct StatusRequestPacket : IRequestPacket
    {
        public byte Id => 0x00;
        public short Length => 32767;
        public ServerModel Model { get; }

        public StatusRequestPacket(ServerModel model)
        {
            Model = model;
        }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.WriteString(Model.ToString());
            return state;
        }
    }
}
