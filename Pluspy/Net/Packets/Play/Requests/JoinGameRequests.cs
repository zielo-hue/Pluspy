using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    public readonly struct JoinGameRequests : IRequestPacket
    {
        public byte Id => 0x26;
        public short Length => 100; // TODO: No idea. I think not needed
        public int EntityId { get; }
        public Gamemode Gamemode { get; }
        public Dimension Dimension { get; }
        public long HashedSeed { get; } // First 8 bytes of the SHA-256 hash of the world's seed.
        public byte MaxPlayers => 0; // Client ignores this value, but still must be written
        public string LevelType { get; } // Must be from WorldType class
        public int ViewDistance { get; }
        public bool ReducedDebugInfo { get; }
        public bool EnableRespawnScreen { get; }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(EntityId);
            stream.Write(Gamemode);
            stream.Write(Dimension);
            stream.Write(HashedSeed);
            stream.Write(MaxPlayers);
            stream.WriteString(LevelType);
            stream.Write(ViewDistance);
            stream.Write(ReducedDebugInfo);
            stream.Write(EnableRespawnScreen);
            return state;
        }
    }
}
