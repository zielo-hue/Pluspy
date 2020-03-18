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
            public static string Digest(string input)
            {
                using var hasher = new SHA1Managed();
                var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

                Array.Reverse(hash);

                var bigNumbah = new BigInteger(hash);

                return bigNumbah < 0 
                    ? "-" + (-bigNumbah).ToString("x").TrimStart('0')
                    : bigNumbah.ToString("x").TrimStart('0');
            }
        }
    }
}
