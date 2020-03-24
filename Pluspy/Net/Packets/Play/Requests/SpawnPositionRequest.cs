using Pluspy.Entities;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    public readonly struct SpawnPositionRequest : IRequestPacket
    {
        public byte Id => 0x4E;
        public short Length => sizeof(ulong);
        public Position Position { get; }

        public SpawnPositionRequest(Position position)
        {
            Position = position;
        }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(Position);
            return state;
        }
    }
}
