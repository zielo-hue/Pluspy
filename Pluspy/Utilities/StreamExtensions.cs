using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Pluspy.Utilities
{
    public static class StreamExtensions
    {
        public static void WriteVarInt(this Stream stream, int value)
        {
            do
            {
                var temp = (byte)(value & 0x7F);

                value >>= 7;

                if (value != 0)
                    temp |= 0x80;

                stream.WriteByte(temp);

            } while (value != 0);
        }

        public static int ReadVarInt(this Stream stream)
        {
            var bytesRead = 0;
            var result = 0;
            byte current;

            do
            {
                current = (byte)stream.ReadByte();
                result |= (current & 0x7F) << (7 * bytesRead);
                bytesRead++;

                if (bytesRead > 5)
                    throw new InvalidOperationException("Data is too large to be a variable integer.");

            } while ((current & 0x80) != 0);

            return result;
        }

        public static T Read<T>(this Stream stream) where T : unmanaged
        {
            var result = default(T);
            var span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref result, 1));

            stream.Read(span);

            return result;
        }

        public static string ReadString(this Stream stream)
        {
            var length = stream.ReadVarInt();
            Span<byte> stringBytes = stackalloc byte[length];

            stream.Read(stringBytes);

            return Encoding.UTF8.GetString(stringBytes);
        }
    }
}