using Pluspy.Attributes;
using Pluspy.Enums;
using Pluspy.Net;
using System;
using System.Security.Cryptography;

namespace Pluspy.Net.Packets.Client
{
    [Clientbound(State.Login)]
    public struct EncryptionKeyResponsePacket : IPacket
    {
        public byte[] SharedSecret { get; private set; }
        public byte[] VerificationToken { get; private set; }

        public EncryptionKeyResponsePacket(byte[] sharedSecret, byte[] verificationToken)
        {
            SharedSecret = sharedSecret;
            VerificationToken = verificationToken;
        }

        public void Decrypt(RSACryptoServiceProvider rsaProvider)
        {
            SharedSecret = rsaProvider.Decrypt(SharedSecret, false);
            VerificationToken = rsaProvider.Decrypt(VerificationToken, false);
        }

        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
        {
            SharedSecret = stream.ReadBytes(stream.Read<short>());
            VerificationToken = stream.ReadBytes(stream.Read<short>());
            return state;
        }

        public readonly State WriteTo(MinecraftStream stream, State state, PacketType type)
        {
            stream.Write((short)SharedSecret.Length);
            stream.WriteSpan(SharedSecret.AsSpan());
            stream.Write((short)VerificationToken.Length);
            stream.WriteSpan(VerificationToken.AsSpan());
            return state;
        }
    }
}
