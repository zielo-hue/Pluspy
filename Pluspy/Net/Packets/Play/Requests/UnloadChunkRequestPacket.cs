using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    public readonly struct UnloadChunkRequestPacket : IRequestPacket
    {
        public byte Id => 0x1E;
        
        public int X { get; }
        public int Z { get; }
        
        public UnloadChunkRequestPacket(int x, int z)
        {
            X = x;
            Z = z;
        }
        
        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(X);
            stream.Write(Z);
            return state;
        }
    }
}