using System.Net.Sockets;

namespace Pluspy.Net.Packets
{
    public readonly struct SpawnPositionPacket : IPacket
    {
        private readonly ulong _position;

        public SpawnPositionPacket(int x, int y, int z)
            => _position = (ulong)(((x & 0x3FFFFFF) << 38) | ((z & 0x3FFFFFF) << 12) | (y & 0xFFF));


        public void WriteTo(NetworkStream stream)
        {
        }
    }
}
