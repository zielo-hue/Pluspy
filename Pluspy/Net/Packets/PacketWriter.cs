using Pluspy.Utilities;
using System;
using System.IO;
using System.Text;

namespace Pluspy.Net.Packets
{
    public ref struct PacketWriter
    {
        private int _id;
        private int _bytesWritten;
        private Span<byte> _packetSpan;

        public PacketWriter(Span<byte> packetSpan, int id)
        {
            _id = id;
            _bytesWritten = 0;
            _packetSpan = packetSpan;
        }

        public void WriteVarInt(int value)
        {
            Extensions.GetVarIntBytes(value, _packetSpan.Slice(_bytesWritten), out int bytesWritten);
            _bytesWritten += bytesWritten;
        }

        public void WriteBytes(Span<byte> bytes)
        {
            bytes.CopyTo(_packetSpan.Slice(_bytesWritten));
            _bytesWritten += bytes.Length;
        }

        public void WriteString(string str)
        {
            int byteLength = Encoding.UTF8.GetByteCount(str);
            WriteVarInt(byteLength);

            Encoding.UTF8.GetBytes(str, _packetSpan.Slice(_bytesWritten));
            _bytesWritten += byteLength;
        }

        public void WriteTo(Stream stream)
        {
            Span<byte> idSpan = stackalloc byte[5];
            Extensions.GetVarIntBytes(_id, idSpan, out int bytesWritten);

            stream.WriteVarInt(_bytesWritten + bytesWritten);
            stream.Write(idSpan.Slice(0, bytesWritten));
            stream.Write(_packetSpan.Slice(0, _bytesWritten));
        }
    }
}
