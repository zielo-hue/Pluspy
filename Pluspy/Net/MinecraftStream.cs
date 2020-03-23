using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Pluspy.Net
{
    public class MinecraftStream : Stream
    {
        private readonly Encoding _encoding;

        public Stream BaseStream { get; }
        public override bool CanRead
            => BaseStream.CanRead;
        public override bool CanSeek
            => BaseStream.CanSeek;
        public override bool CanWrite
            => BaseStream.CanWrite;
        public override long Length
            => BaseStream.Length;
        public override long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public MinecraftStream(Stream stream)
        {
            _encoding = Encoding.UTF8;
            BaseStream = stream;
        }

        public override void Flush()
            => BaseStream.Flush();
        public override int Read(byte[] buffer, int offset, int count)
            => BaseStream.Read(buffer.AsSpan(offset, count));
        public override int Read(Span<byte> buffer)
            => BaseStream.Read(buffer);
        public override long Seek(long offset, SeekOrigin origin)
            => BaseStream.Seek(offset, origin);
        public override void SetLength(long value)
            => BaseStream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count)
            => BaseStream.Write(buffer.AsSpan(offset, count));
        public override void Write(ReadOnlySpan<byte> buffer)
            => BaseStream.Write(buffer);

        public int ReadVarInt()
            => ReadVarInt(out _);

        public int ReadVarInt(out int bytesRead)
        {
            bytesRead = 0;

            var result = 0;
            byte current;

            do
            {
                current = (byte)BaseStream.ReadByte();
                result |= (current & 0x7F) << (7 * bytesRead++);

                if (bytesRead > 5)
                    throw new InvalidOperationException("Data is too large to be a variable integer.");

            } while ((current & 0x80) != 0);

            return result;
        }

        public void WriteVarInt(int value)
            => WriteVarInt(value, out _);

        public void WriteVarInt(int value, out int bytesWritten)
        {
            bytesWritten = 0;

            do
            {
                var temp = (byte)(value & 0x7F);

                value >>= 7;

                if (value != 0)
                    temp |= 0x80;

                BaseStream.WriteByte(temp);
                bytesWritten++;

            } while (value != 0);
        }

        public T Read<T>() where T : struct
        {
            Span<byte> span = stackalloc byte[Unsafe.SizeOf<T>()];

            BaseStream.Read(span);
            return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(span));
        }

        public void Write<T>(T value) where T : struct
        {
            var span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1));
            BaseStream.Write(span);
        }

        public byte[] ReadBytes(int length)
        {
            if (length == 0) 
                return Array.Empty<byte>();

            byte[] array = null;

            try
            {
                array = ArrayPool<byte>.Shared.Rent(length);
                BaseStream.Read(array);
                return array;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public void WriteSpan(Span<byte> span)
        {
            foreach (var item in span)
                WriteByte(item);
        }

        public Memory<byte> ReadMemory(int length)
        {
            if (length == 0)
                return Array.Empty<byte>();

            using var memoryRent = MemoryPool<byte>.Shared.Rent(length);

            BaseStream.Read(memoryRent.Memory.Span);
            return memoryRent.Memory;
        }

        public string ReadString()
        {
            var length = ReadVarInt();
            Span<byte> span = stackalloc byte[length];

            BaseStream.Read(span);
            return _encoding.GetString(span);
        }

        public void WriteString(string value)
        {
            WriteVarInt(_encoding.GetByteCount(value));

            if (value.Length > 0)
                BaseStream.Write(_encoding.GetBytes(value));
        }
    }
}
