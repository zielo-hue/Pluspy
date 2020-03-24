using Pluspy.Attributes;
using Pluspy.Enums;
using System;

namespace Pluspy.Net.Packets.Requests
{
    [PacketState(State.Login)]
    public readonly struct EncryptionRequestPacket : IRequestPacket
    {
        public byte Id => 0x01;
        public string ServerId { get; }
        public Memory<byte> PublicKey { get; }
        public Memory<byte> VerifyToken { get; }

        public EncryptionRequestPacket(Memory<byte> publicKey, Memory<byte> verifyToken) : this(null, publicKey, verifyToken)
        {
        }

        public EncryptionRequestPacket(string serverId, Memory<byte> publicKey, Memory<byte> verifyToken)
        {
            ServerId = serverId;
            PublicKey = publicKey;
            VerifyToken = verifyToken;
        }

        public State WriteTo(MinecraftStream stream, State state)
        {
            if (ServerId is object)
                stream.WriteString(ServerId);
            else
            {
                stream.WriteVarInt(20);
                stream.WriteSpan(stackalloc byte[20]);
            }
            
            stream.WriteVarInt(PublicKey.Length);
            stream.WriteSpan(PublicKey.Span);
            stream.WriteVarInt(VerifyToken.Length);
            stream.WriteSpan(VerifyToken.Span);
            return state;
        }
    }
}
