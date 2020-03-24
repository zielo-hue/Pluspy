namespace Pluspy.Net
{
    public interface IPacketModel<TPacket> where TPacket : IPacket
    {
        TPacket ToPacket();
    }
}
