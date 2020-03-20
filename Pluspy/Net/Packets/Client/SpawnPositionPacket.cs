using Pluspy.Enums;
using System.Net.Sockets;

namespace Pluspy.Net.Packets
{
    public readonly struct SpawnPositionPacket : IPacket
    {
        private readonly ulong _position;

        public SpawnPositionPacket(int x, int y, int z)
            => _position = (ulong)(((x & 0x3FFFFFF) << 38) | ((z & 0x3FFFFFF) << 12) | (y & 0xFFF));

        public State ReadFrom(NetworkStream stream, State state, PacketType type)
        {
            throw new System.NotImplementedException();
        }

        public State WriteTo(NetworkStream stream, State state, PacketType type)
        {
            throw new System.NotImplementedException();
        }
    }
}
