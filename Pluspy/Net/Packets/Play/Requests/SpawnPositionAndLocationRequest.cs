using Pluspy.Enums;
using System.Runtime.CompilerServices;

namespace Pluspy.Net.Packets.Requests
{
    public readonly struct SpawnPositionAndLocationRequest : IRequestPacket
    {
        public byte Id => 0x12;
        public short Length => sizeof(double) * 3 + sizeof(float) * 2 + sizeof(bool);
        public double X { get; }
        public double FeetY { get; }
        public double Z { get; }
        public float Yaw { get; }
        public float Pitch { get; }
        public bool OnGround { get; }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(X);
            stream.Write(FeetY);
            stream.Write(Z);
            stream.Write(Yaw);
            stream.Write(Pitch);
            stream.Write(OnGround);
            return state;
        }
    }
}
