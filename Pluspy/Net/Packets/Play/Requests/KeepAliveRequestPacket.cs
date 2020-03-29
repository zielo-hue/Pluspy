using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    public readonly struct KeepAliveRequestPacket : IRequestPacket
    {
        public byte Id => 0x21;
        
        public long KeepAliveID { get; }

        public KeepAliveRequestPacket(long keepAliveId)
        {
            KeepAliveID = keepAliveId;
        }
        
        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(KeepAliveID);
            return state;
        }
    }
}