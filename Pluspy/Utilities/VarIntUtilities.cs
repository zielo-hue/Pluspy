using System;

namespace Pluspy.Utilities
{
    public static class VarIntUtilities
    {
        public static int GetBytes(int value, Span<byte> destination)
        {
            var index = 0;

            do
            {
                var temp = (byte)(value & 0x7F);

                value >>= 7;

                if (value != 0)
                    temp |= 0x80;

                destination[index] = temp;
                index++;

            } while (value != 0);

            return index;
        }

        public static int GetLength(int value)
        {
            var length = 0;
            var temp = (uint)value;

            do
            {
                length++;
                temp >>= 7;

            } while ((temp & 0xFFFFFF80u) != 0u);

            return length;
        }
    }
}
