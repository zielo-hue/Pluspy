using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Pluspy.Utilities
{
    public static class Encryption
    {
        public static class SHA1
        {
            public static string Digest(ReadOnlySpan<byte> input)
            {
                using var sha1 = new SHA1Managed();
                Span<byte> outputSpan = stackalloc byte[20];
                sha1.TryComputeHash(input, outputSpan, out _);
                outputSpan.Reverse();

                var hashInteger = new BigInteger(outputSpan);

                return hashInteger < 0
                    ? "-" + (-hashInteger).ToString("x").TrimStart('0')
                    : hashInteger.ToString("x").TrimStart('0');
            }
        }
    }
}
