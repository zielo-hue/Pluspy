using Pluspy.Attributes;
using Pluspy.Enums;

namespace Pluspy.Net.Packets
{
    [PacketState(State.Status)]
    public struct StatusPingPacket : IResponsePacket, IRequestPacket
    {
        public byte Id => 0x01;
        public short Length => sizeof(long);
        public long Time { get; private set; }

        public StatusPingPacket(long time)
        {
            Time = time;
        }

        public State ReadFrom(MinecraftStream stream, State state)
        {
            Time = stream.Read<long>();
            return state;
        }

        public readonly State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(Time);
            return state;
        }
    }
}
