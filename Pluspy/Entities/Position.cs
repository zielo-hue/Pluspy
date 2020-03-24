using System;

namespace Pluspy.Entities
{
    public readonly struct Position : IComparable<ulong>, IEquatable<ulong>
    {
        private readonly ulong _value;

        public Position(ulong value)
        {
            _value = value;
        }

        public Position(int x, int y, int z)
        {
            _value = (((ulong)x & 0x3FFFFFF) << 38) | (((ulong)z & 0x3FFFFFF) << 12) | (ulong)y & 0xFFF;
        }

        public int CompareTo(ulong other)
            => _value.CompareTo(other);

        public bool Equals(ulong other)
            => _value == other;

        public override bool Equals(object obj)
            => obj is Position position && position._value == _value
            || obj is ulong uint64 && uint64 == _value;

        public override int GetHashCode()
            => _value.GetHashCode();

        public override string ToString()
            => _value.ToString();

        public static implicit operator Position(ulong value)
            => new Position(value);

        public static implicit operator ulong(Position position)
            => position._value;

        public static implicit operator (int, int, int)(Position position)
            => ((int)(position._value >> 38), (int)(position._value & 0xFFF), (int)(position._value << 26 >> 38));

        public static implicit operator Position((int X, int Y, int Z) coords)
            => new Position(coords.X, coords.Y, coords.Z);
    }
}
