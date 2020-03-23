using Pluspy.Attributes;
using Pluspy.Enums;
using System;
using System.Security.Cryptography;

namespace Pluspy.Net.Packets.Server
{
    [Serverbound(State.Login)]
    public struct EncryptionKeyRequestPacket : IPacket
    {
    //    public string ServerId { get; private set; }
        public Memory<byte> PublicKey { get; private set; }
        public Memory<byte> VerifyToken { get; private set; }

        public EncryptionKeyRequestPacket(Memory<byte> publicKey, Memory<byte> verifyToken) : this(null, publicKey, verifyToken)
        {
        }

        public EncryptionKeyRequestPacket(string serverId, Memory<byte> publicKey, Memory<byte> verifyToken)
        {
           // ServerId = serverId;
            PublicKey = publicKey;
            VerifyToken = verifyToken;
        }

        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
        {
           // ServerId = stream.ReadString();
            PublicKey = stream.ReadMemory(stream.Read<short>());
            VerifyToken = stream.ReadMemory(stream.Read<short>());
            return state;
        }

        public readonly State WriteTo(MinecraftStream stream, State state, PacketType type)
        {
         //   if (ServerId is object) 
         //       stream.WriteString(ServerId);

            stream.WriteVarInt(PublicKey.Length);
            stream.WriteSpan(PublicKey.Span);
            stream.WriteVarInt(VerifyToken.Length);
            stream.WriteSpan(VerifyToken.Span);
            return state;
        }
    }
}
