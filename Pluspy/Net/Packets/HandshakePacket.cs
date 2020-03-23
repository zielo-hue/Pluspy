using Pluspy.Attributes;
using Pluspy.Enums;
using Pluspy.Net;

namespace Pluspy.Net.Packets
{
    [Clientbound(State.Handshake)]
    [Serverbound(State.Handshake)]
    public struct HandshakePacket : IPacket
    {
        public int ProtocolVersion { get; private set; }
        public string ServerHostname { get; private set; }
        public ushort ServerPort { get; private set; }
        public State NextState { get; private set; }

        public HandshakePacket(int protocolVersion, string hostname, ushort port, State nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerHostname = hostname;
            ServerPort = port;
            NextState = nextState;
        }

        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
        {
            ProtocolVersion = stream.ReadVarInt();
            ServerHostname = stream.ReadString();
            ServerPort = stream.Read<ushort>();
            NextState = stream.ReadVarInt() == 1 ? State.Status : State.Login;
            return NextState;
        }

        public readonly State WriteTo(MinecraftStream stream, State state, PacketType type)
        {
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerHostname);
            stream.Write(ServerPort);
            stream.WriteVarInt((int)NextState);
            return NextState;
        }
    }
}
