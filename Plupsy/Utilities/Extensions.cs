using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Pluspy.Utilities
{
    public static class Extensions
    {
        public static void WriteVarInt(this BinaryWriter writer, int value)
        {
            do
            {
                var temp = (byte)(value & 0x7F);

                value >>= 7;

                if (value != 0)
                    temp |= 0x80;

                writer.Write(temp);

            } while (value != 0);
        }

        public static int ReadVarInt(this NetworkStream stream)
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

        public static Span<byte> GetBytes(this int value)
        {
            var bytes = BitConverter.GetBytes(value).AsSpan();

            if (BitConverter.IsLittleEndian)
                bytes.Reverse();

            return bytes;
        }

        public static T Read<T>(this NetworkStream stream, bool isBigEndian = true) where T : struct
        {
            var result = default(T);
            var span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref result, 1));

            stream.Read(span);

            if (isBigEndian && BitConverter.IsLittleEndian) 
                span.Reverse();

            return result;
        }

        public static string ReadString(this NetworkStream stream)
        {
            var length = stream.ReadVarInt();
            Span<byte> stringBytes = stackalloc byte[length];

            stream.Read(stringBytes);

            return Encoding.UTF8.GetString(stringBytes);
        }

        public static void GetVarIntBytes(this int value, Span<byte> destination, out int bytesWritten)
        {
            Span<byte> buffer = stackalloc byte[5];
            var index = 0;

            do
            {
                var temp = (byte)(value & 0x7F);

                value >>= 7;

                if (value != 0)
                    temp |= 0x80;

                buffer[index] = temp;
                index++;

            } while (value != 0);

            buffer[..index].CopyTo(destination);
            bytesWritten = index;
        }

        public static void GetBytes(this ulong value, Span<byte> destination)
        {
            var buffer = BitConverter.GetBytes(value).AsSpan();

            if (BitConverter.IsLittleEndian) 
                buffer.Reverse();

            buffer.CopyTo(destination);
        }

        public static bool TryGetVarIntBytes(this int value, Span<byte> destination)
        {
            Span<byte> buffer = stackalloc byte[5];
            var index = 0;

            do
            {
                var temp = (byte)(value & 0x7F);

                value >>= 7;

                if (value != 0)
                    temp |= 0x80;

                buffer[index] = temp;
                index++;

            } while (value != 0);

            return buffer[..index].TryCopyTo(destination);
        }

        public static Span<byte> GetBytes(this ulong value)
        {
            var bytes = BitConverter.GetBytes(value).AsSpan();

            if (BitConverter.IsLittleEndian) 
                bytes.Reverse();

            return bytes;
        }

        public static Span<byte> GetBytes(this long value)
        {
            var bytes = BitConverter.GetBytes(value).AsSpan();

            if (BitConverter.IsLittleEndian)
                bytes.Reverse();
            
            return bytes;
        }

        public static byte GetByte(this bool value)
            => (byte)(value ? 1 : 0);

        public static void GetStringBytes(string data, Span<byte> destination)
        {
            Span<byte> intBytes = stackalloc byte[5];
            GetVarIntBytes(data.Length, intBytes, out int intBytesLength);

            var r = intBytes.Slice(0, intBytesLength).TryCopyTo(destination);
            Encoding.UTF8.GetBytes(data, destination.Slice(intBytesLength));
        }

        public static Span<byte> GetBytes(this string data)
        {
            Span<byte> bytes = stackalloc byte[5];

            GetVarIntBytes(data.Length, bytes, out int intBytesLength);

            var destination = new byte[intBytesLength + Encoding.UTF8.GetByteCount(data)].AsSpan();

            bytes[..intBytesLength].CopyTo(destination);
            Encoding.UTF8.GetBytes(data, destination);

            return destination;
        }

        public static Span<byte> GetBytes(this float value)
        {
            var bytes = BitConverter.GetBytes(value).AsSpan();

            if (BitConverter.IsLittleEndian) 
                bytes.Reverse();

            return bytes;
        }

        public static Span<byte> GetBytes(double value)
        {
            var bytes = BitConverter.GetBytes(value).AsSpan();

            if (BitConverter.IsLittleEndian) 
                bytes.Reverse();

            return bytes;
        }
    }
}