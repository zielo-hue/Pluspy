using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Pluspy
{
    public static class Utilities
    {
        public static class SHA1
        {
            public static string Digest(ReadOnlySpan<byte> input)
            {
                using var sha1 = new SHA1Managed();
                Span<byte> outputSpan = stackalloc byte[20];
                var hash = sha1.TryComputeHash(input, outputSpan, out _);

                outputSpan.Reverse();

                var hashInteger = new BigInteger(outputSpan);

                return hashInteger < 0 
                    ? "-" + (-hashInteger).ToString("x").TrimStart('0')
                    : hashInteger.ToString("x").TrimStart('0');
            }
        }
    }
}
