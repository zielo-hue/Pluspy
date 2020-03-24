using Pluspy.Utilities;
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace Pluspy.Net
{
    public sealed class LazyStream : Stream
    {
        private readonly Stream _stream;
        private readonly MemoryStream _memoryStream;

        public long PendingWrites
            => _memoryStream.Position;
        public override bool CanRead 
            => _stream.CanRead;
        public override bool CanSeek 
            => _stream.CanSeek;
        public override bool CanWrite
            => _stream.CanWrite;
        public override long Length 
            => _stream.Length;
        public override long Position 
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }
        public bool WriteImmediately { get; set; }

        public LazyStream(Stream stream) : this(stream, 512) 
        {
        }

        public LazyStream(Stream stream, int bufferSize)
        {
            _stream = stream;
            _memoryStream = new MemoryStream(bufferSize);
        }

        public override void Flush()
        {
            int packetLength = (int)_memoryStream.Position;
            _stream.WriteVarInt(packetLength);
            _stream.Write(_memoryStream.GetBuffer().AsSpan(0, packetLength));
            _memoryStream.Position = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
            => _stream.Read(buffer.AsSpan(offset, count));

        public override int Read(Span<byte> buffer)
            => _stream.Read(buffer);

        public override long Seek(long offset, SeekOrigin origin)
            => _stream.Seek(offset, origin);

        public override void SetLength(long value)
            => _stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
            => Write(buffer.AsSpan(offset, count));
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            if (WriteImmediately)
                _stream.Write(buffer);
            else
                _memoryStream.Write(buffer);
        }
    }
}
