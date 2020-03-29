using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    public readonly struct ChangeGameStateRequestPacket : IRequestPacket
    {
        public byte Id => 0x1F;
        
        public byte Reason { get; }
        public float Value { get; }

        public ChangeGameStateRequestPacket(byte reason, float value)
        {
            Reason = reason;
            Value = value;
        }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(Reason);
            stream.Write(Value);
            return state;
        }
    }
}