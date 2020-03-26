using Pluspy.Enums;

namespace Pluspy.Net.Packets.Play.Requests
{
    public struct UnloadPositionRequest : IRequestPacket
    {
        public byte Id => 0x1E;
        
        public int X { get; set; }
        public int Y { get; set; }
        
        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(X);
            stream.Write(Y);
            return state;
        }
    }
}