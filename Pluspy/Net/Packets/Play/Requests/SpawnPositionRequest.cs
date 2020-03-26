using Pluspy.Entities;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    public readonly struct SpawnPositionRequest : IRequestPacket
    {
        public byte Id => 0x43;
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
