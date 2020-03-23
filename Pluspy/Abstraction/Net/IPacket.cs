namespace Pluspy.Net
{
    public interface IPacket
    {
        byte Id { get; }
        short Length { get; }
        void Prepare(int packetLength, byte packetId) { }
    }
}
