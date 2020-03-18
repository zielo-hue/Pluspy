using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Pluspy.Net.Packets.Client
{
    public readonly ref struct EncryptionRequest
    {
        private readonly Span<byte> _publicKey;
        private readonly Span<byte> _verifyToken;

        public EncryptionRequest(Span<byte> publicKey, Span<byte> verifyToken)
        {
            _publicKey = publicKey;
            _verifyToken = verifyToken;
        }

        public void WriteTo(NetworkStream stream)
        {
            Span<byte> packetBytes = stackalloc byte[_publicKey.Length + _verifyToken.Length + 256];
            PacketWriter writer = new PacketWriter(packetBytes, 0x01);

            writer.WriteVarInt(20);
            writer.WriteBytes(stackalloc byte[20]);

            writer.WriteVarInt(_publicKey.Length);
            writer.WriteBytes(_publicKey);

            writer.WriteVarInt(_verifyToken.Length);
            writer.WriteBytes(_verifyToken);

            writer.WriteTo(stream);
        }
    }
}
