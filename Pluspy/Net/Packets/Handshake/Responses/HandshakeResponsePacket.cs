using Pluspy.Attributes;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Responses
{
    [PacketState(State.Handshake)]
    public struct HandshakeResponsePacket : IResponsePacket
    {
        public byte Id => 0x00;
        public short Length => default;
        public int ProtocolVersion { get; private set; }
        public string ServerHostname { get; private set; }
        public ushort ServerPort { get; private set; }
        public State NextState { get; private set; }

        public HandshakeResponsePacket(int protocolVersion, string hostname, ushort port, State nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerHostname = hostname;
            ServerPort = port;
            NextState = nextState;
        }

        public State ReadFrom(MinecraftStream stream, State state)
        {
            ProtocolVersion = stream.ReadVarInt();
            ServerHostname = stream.ReadString();
            ServerPort = stream.Read<ushort>();
            NextState = (State)stream.ReadVarInt();
            return NextState;
        }
    }
}
