using Pluspy.Enums;
using System;
using System.Net.Sockets;

namespace Pluspy.Net.Packets.Client
{
    public struct EncryptionRequest : IPacket
    {
        private readonly Memory<byte> _publicKey;
        private readonly Memory<byte> _verifyToken;

        public EncryptionRequest(Memory<byte> publicKey, Memory<byte> verifyToken)
        {
            _publicKey = publicKey;
            _verifyToken = verifyToken;
        }

        public State ReadFrom(NetworkStream stream, State state, PacketType type)
            => state;

        public State WriteTo(NetworkStream stream, State state, PacketType type)
        {
            Span<byte> packetBytes = stackalloc byte[_publicKey.Length + _verifyToken.Length + 256];
            var writer = new PacketWriter(packetBytes, 0x01);

            writer.WriteVarInt(20);
            writer.WriteBytes(stackalloc byte[20]);

            writer.WriteVarInt(_publicKey.Length);
            writer.WriteBytes(_publicKey.Span);

            writer.WriteVarInt(_verifyToken.Length);
            writer.WriteBytes(_verifyToken.Span);

            writer.WriteTo(stream);
            return state;
        }
    }
}
