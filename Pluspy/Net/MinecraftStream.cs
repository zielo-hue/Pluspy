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
        public Stream BaseReadStream { get; }

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

        public MinecraftStream(Stream writeStream, Stream readStream)
        {
            _encoding = Encoding.UTF8;
            BaseStream = writeStream;
            BaseReadStream = readStream;
        }

        public override void Flush()
            => BaseStream.Flush();
        public override int Read(byte[] buffer, int offset, int count)
            => BaseReadStream.Read(buffer.AsSpan(offset, count));
        public override int Read(Span<byte> buffer)
            => BaseReadStream.Read(buffer);
        public override long Seek(long offset, SeekOrigin origin)
            => BaseStream.Seek(offset, origin);
        public override void SetLength(long value)
            => BaseStream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count)
            => BaseStream.Write(buffer.AsSpan(offset, count));
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            BaseStream.Write(buffer);
        }

        public int ReadVarInt()
            => ReadVarInt(out _);

        public int ReadVarInt(out int bytesRead)
        {
            bytesRead = 0;

            var result = 0;
            byte current;

            do
            {
                current = (byte)BaseReadStream.ReadByte();
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

            BaseReadStream.Read(span);
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

            byte[] array = new byte[length];
            BaseReadStream.Read(array);
            return array;
        }

        public void WriteSpan(Span<byte> span)
        {
            BaseStream.Write(span);
        }

        public string ReadString()
        {
            var length = ReadVarInt();
            Span<byte> span = stackalloc byte[length];

            BaseReadStream.Read(span);
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
