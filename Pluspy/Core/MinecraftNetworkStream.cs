using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Pluspy.Core
{
    public sealed class MinecraftNetworkStream : Stream
    {
        public override bool CanRead 
            => Stream.CanRead;
        public override bool CanSeek
            => Stream.CanSeek;
        public override bool CanWrite
            => Stream.CanWrite;
        public override long Length
            => Stream.Length;

        public override long Position 
        {
            get => Stream.Position;
            set => Stream.Position = value;
        }

        public NetworkStream Stream { get; }

        public MinecraftNetworkStream(NetworkStream stream)
        {
            Stream = stream;
        }

        public int ReadVarInt()
        {
            var result = 0u;
            var length = 0;

            while (true)
            {
                var current = ReadUInt8();

                result |= (current & 0x7Fu) << length++ * 7;

                if (length > 5)
                    throw new InvalidDataException("VarInt may not be longer than 28 bits.");

                if ((current & 0x80) != 128)
                    break;
            }

            return (int)result;
        }

        public int ReadVarInt(out int length)
        {
            var result = 0u;

            length = 0;

            while (true)
            {
                var current = ReadUInt8();

                result |= (current & 0x7Fu) << length++ * 7;

                if (length > 5)
                    throw new InvalidDataException("VarInt may not be longer than 60 bits.");

                if ((current & 0x80) != 128)
                    break;
            }

            return (int)result;
        }

        public void WriteVarInt(uint value)
        {
            while (true)
            {
                if ((value & 0xFFFFFF80u) == 0)
                {
                    WriteUInt8((byte)value);
                    break;
                }

                WriteUInt8((byte)(value & 0x7F | 0x80));
                value >>= 7;
            }
        }

        public void WriteVarInt(uint value, out int length)
        {
            length = 0;

            while (true)
            {
                length++;

                if ((value & 0xFFFFFF80u) == 0)
                {
                    WriteUInt8((byte)value);
                    break;
                }

                WriteUInt8((byte)(value & 0x7F | 0x80));
                value >>= 7;
            }
        }

        public static int GetVarIntLength(uint value)
        {
            var length = 0;

            while (true)
            {
                length++;

                if ((value & 0xFFFFFF80u) == 0)
                    break;

                value >>= 7;
            }

            return length;
        }

        public byte ReadUInt8()
        {
            var value = Stream.ReadByte();

            if (value == -1)
                throw new EndOfStreamException();

            return (byte)value;
        }

        public void WriteUInt8(byte value)
            => WriteByte(value);

        public sbyte ReadInt8()
            => (sbyte)ReadUInt8();

        public void WriteInt8(sbyte value)
            => WriteUInt8((byte)value);

        public ushort ReadUInt16()
            => (ushort)((ReadUInt8() << 8) | ReadUInt8());

        public void WriteUInt16(ushort value)
        {
            Span<byte> span = stackalloc byte[2];
            var bytes = new Bytes
            {
                Value = value
            };

            span[0] = bytes.Byte0;
            span[1] = bytes.Byte1;
            Write(span);
        }

        public short ReadInt16()
            => (short)ReadUInt16();

        public void WriteInt16(short value)
            => WriteUInt16((ushort)value);


        public uint ReadUInt32()
            => (uint)((ReadUInt8() << 24) 
            |  (ReadUInt8() << 16) 
            |  (ReadUInt8() << 8) 
            |   ReadUInt8());

        public void WriteUInt32(uint value)
        {
            Span<byte> span = stackalloc byte[4];
            var bytes = new Bytes
            {
                Value = value
            };

            span[0] = bytes.Byte0;
            span[1] = bytes.Byte1;
            span[2] = bytes.Byte2;
            span[3] = bytes.Byte3;
            Write(span);
        }

        public int ReadInt32()
            => (int)ReadUInt32();

        public void WriteInt32(int value)
            => WriteUInt32((uint)value);

        public ulong ReadUInt64()
            => ((ulong)ReadUInt8() << 56) 
            |  ((ulong)ReadUInt8() << 48) 
            |  ((ulong)ReadUInt8() << 40) 
            |  ((ulong)ReadUInt8() << 32) 
            |  ((ulong)ReadUInt8() << 24) 
            |  ((ulong)ReadUInt8() << 16) 
            |  ((ulong)ReadUInt8() << 8) 
            |  ReadUInt8();

        public void WriteUInt64(ulong value)
        {
            Span<byte> span = stackalloc byte[4];
            var bytes = new Bytes
            {
                Value = value
            };

            span[0] = bytes.Byte0;
            span[1] = bytes.Byte1;
            span[2] = bytes.Byte2;
            span[3] = bytes.Byte3;
            span[4] = bytes.Byte4;
            span[5] = bytes.Byte5;
            span[6] = bytes.Byte6;
            span[7] = bytes.Byte7;
            Write(span);
        }

        public long ReadInt64()
            => (long)ReadUInt64();

        public void WriteInt64(long value)
            => WriteUInt64((ulong)value);

        public Span<byte> ReadUInt8Array(int length)
        {
            if (length == 0)
                return Array.Empty<byte>();

            var result = new byte[length];
            var n = length;

            while (true)
            {
                n -= Read(result, length - n, n);

                if (n == 0)
                    break;
            }

            return result;
        }

        public void WriteUInt8Array(ReadOnlySpan<byte> value)
            => Write(value);

        public Span<sbyte> ReadInt8Array(int length)
            => MemoryMarshal.Cast<byte, sbyte>(ReadUInt8Array(length));

        public void WriteInt8Array(ReadOnlySpan<sbyte> value)
            => Write(MemoryMarshal.Cast<sbyte, byte>(value));

        public Span<ushort> ReadUInt16Array(int length)
        {
            if (length == 0)
                return Array.Empty<ushort>();

            var result = new ushort[length];

            for (int i = 0; i < length; i++)
                result[i] = ReadUInt16();

            return result;
        }

        public void WriteUInt16Array(ReadOnlySpan<ushort> value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteUInt16(value[i]);
        }

        public Span<short> ReadInt16Array(int length)
            => MemoryMarshal.Cast<ushort, short>(ReadUInt16Array(length));

        public void WriteInt16Array(ReadOnlySpan<short> value)
            => WriteUInt16Array(MemoryMarshal.Cast<short, ushort>(value));

        public Span<uint> ReadUInt32Array(int length)
        {
            if (length == 0)
                return Array.Empty<uint>();

            var result = new uint[length];

            for (int i = 0; i < length; i++)
                result[i] = ReadUInt32();

            return result;
        }

        public void WriteUInt32Array(ReadOnlySpan<uint> value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteUInt32(value[i]);
        }

        public Span<int> ReadInt32Array(int length)
            => MemoryMarshal.Cast<uint, int>(ReadUInt32Array(length));

        public void WriteInt32Array(ReadOnlySpan<int> value)
            => WriteUInt32Array(MemoryMarshal.Cast<int, uint>(value));

        public Span<ulong> ReadUInt64Array(int length)
        {
            if (length == 0)
                return Array.Empty<ulong>();

            var result = new ulong[length];

            for (int i = 0; i < length; i++)
                result[i] = ReadUInt64();

            return result;
        }

        public void WriteUInt64Array(ReadOnlySpan<ulong> value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteUInt64(value[i]);
        }

        public Span<long> ReadInt64Array(int length)
            => MemoryMarshal.Cast<ulong, long>(ReadUInt64Array(length));

        public void WriteInt64Array(ReadOnlySpan<long> value)
            => WriteUInt64Array(MemoryMarshal.Cast<long, ulong>(value));

        public float ReadSingle()
        {
            var value = ReadUInt32();
            return Unsafe.As<uint, float>(ref value);
        }

        public void WriteSingle(float value)
            => WriteUInt32(Unsafe.As<float, uint>(ref value));

        public double ReadDouble()
        {
            var value = ReadUInt64();
            return Unsafe.As<ulong, double>(ref value);
        }

        public void WriteDouble(double value)
            => WriteUInt64(Unsafe.As<double, ulong>(ref value));

        public bool ReadBoolean()
            => ReadUInt8() != 0;

        public void WriteBoolean(bool value)
            => WriteUInt8((byte)(value ? 1 : 0));

        public string ReadString()
        {
            var length = ReadVarInt();

            if (length == 0) 
                return string.Empty;

            var data = ReadUInt8Array(length);

            return Encoding.UTF8.GetString(data);
        }

        public void WriteString(string value)
        {
            WriteVarInt((uint)Encoding.UTF8.GetByteCount(value));

            if (value.Length > 0)
                WriteUInt8Array(Encoding.UTF8.GetBytes(value));
        }
        
        public override void Flush()
            => Stream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
            => Stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin)
            => Stream.Seek(offset, origin);

        public override void SetLength(long value)
            => Stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
            => Stream.Write(buffer, offset, count);

        [StructLayout(LayoutKind.Explicit)]
        private struct Bytes
        {
            [FieldOffset(0)]
            public readonly byte Byte0;
            [FieldOffset(1)]
            public readonly byte Byte1;
            [FieldOffset(2)]
            public readonly byte Byte2;
            [FieldOffset(3)]
            public readonly byte Byte3;
            [FieldOffset(4)]
            public readonly byte Byte4;
            [FieldOffset(5)]
            public readonly byte Byte5;
            [FieldOffset(6)]
            public readonly byte Byte6;
            [FieldOffset(7)]
            public readonly byte Byte7;
            [FieldOffset(0)]
            public ulong Value;
        }
    }
}
