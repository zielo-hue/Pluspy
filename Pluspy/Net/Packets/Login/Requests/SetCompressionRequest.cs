using Pluspy.Attributes;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    [PacketState(State.Login)]
    public readonly struct SetCompressionRequest : IRequestPacket
    {
        public byte Id => 0x03;
        public short Length => sizeof(int);

        public int Threshold { get; }

        public SetCompressionRequest(int threshold)
        {
            Threshold = threshold;
        }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(Threshold);
            return state;
        }
    }
}
