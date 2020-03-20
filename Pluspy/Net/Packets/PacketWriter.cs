using Pluspy.Utilities;
using System;
using System.IO;
using System.Text;

namespace Pluspy.Net.Packets
{
    public ref struct PacketWriter
    {
        private readonly int _id;
        private readonly Span<byte> _packetSpan;
        private int _bytesWritten;

        public PacketWriter(Span<byte> packetSpan, int id)
        {
            _id = id;
            _bytesWritten = 0;
            _packetSpan = packetSpan;
        }

        public void WriteVarInt(int value)
            => _bytesWritten += VarIntUtilities.GetBytes(value, _packetSpan[_bytesWritten..]);

        public void WriteBytes(Span<byte> bytes)
        {
            bytes.CopyTo(_packetSpan[_bytesWritten..]);
            _bytesWritten += bytes.Length;
        }

        public void WriteString(string str)
        {
            var byteLength = Encoding.UTF8.GetByteCount(str);
            WriteVarInt(byteLength);

            Encoding.UTF8.GetBytes(str, _packetSpan[_bytesWritten..]);
            _bytesWritten += byteLength;
        }

        public readonly void WriteTo(Stream stream)
        {
            Span<byte> idSpan = stackalloc byte[5];
            var bytesWritten = VarIntUtilities.GetBytes(_id, idSpan);

            stream.WriteVarInt(_bytesWritten + bytesWritten);
            stream.Write(idSpan[..bytesWritten]);
            stream.Write(_packetSpan[..bytesWritten]);
        }
    }
}
