using Pluspy.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Pluspy.Net.Packets.Server
{
    public struct EncryptionResponse
    {
        public byte[] SharedSecret { get; private set; }
        public byte[] VerifyToken { get; private set; }

        public EncryptionResponse(NetworkStream stream)
        {
            SharedSecret = new byte[stream.ReadVarInt()];
            stream.Read(SharedSecret);

            VerifyToken = new byte[stream.ReadVarInt()];
            stream.Read(VerifyToken);
        }

        public void Decrypt(RSACryptoServiceProvider rsaProvider)
        {
            SharedSecret = rsaProvider.Decrypt(SharedSecret, false);
            VerifyToken = rsaProvider.Decrypt(VerifyToken, false);
        }
    }
}
