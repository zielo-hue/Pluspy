namespace Pluspy.Net
{
    public interface IPacket
    {
        byte Id { get; }
        void Prepare(int packetLength, byte packetId) { }
    }
}
