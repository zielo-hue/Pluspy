﻿using Pluspy.Attributes;
using Pluspy.Enums;
using System.Security.Cryptography;

namespace Pluspy.Net.Packets.Responses
{
    [PacketState(State.Login)]
    public struct EncryptionResponsePacket : IResponsePacket
    {
        public byte Id => 0x01;
        public short Length => (short)(SharedSecret.Length + VerificationToken.Length);
        public byte[] SharedSecret { get; private set; }
        public byte[] VerificationToken { get; private set; }

        public void Decrypt(RSACryptoServiceProvider rsaProvider)
        {
            SharedSecret = rsaProvider.Decrypt(SharedSecret, false);
            VerificationToken = rsaProvider.Decrypt(VerificationToken, false);
        }

        public State ReadFrom(MinecraftStream stream, State state)
        {
            SharedSecret = stream.ReadBytes(stream.Read<short>());
            VerificationToken = stream.ReadBytes(stream.Read<short>());
            return state;
        }
    }
}
