using Pluspy.Net;

namespace Pluspy.Abstraction.Net
{
    public interface IPacketModel<TPacket> where TPacket : IPacket
    {
        TPacket ToPacket();
    }
}
