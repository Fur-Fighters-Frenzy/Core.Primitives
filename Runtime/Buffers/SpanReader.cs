using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Validosik.Core.Primitives.Buffers
{
    /// <summary>
    /// Sequential reader over a contiguous ReadOnlySpan&lt;byte&gt;.
    /// All reads are little-endian (for fixed-width numbers).
    /// </summary>
    public ref struct SpanReader
    {
        private ReadOnlySpan<byte> _span;
        private int _pos;

        public SpanReader(ReadOnlySpan<byte> span)
        {
            _span = span;
            _pos = 0;
        }

        /// <summary>Bytes remaining to read.</summary>
        public int Remaining => _span.Length - _pos;

        /// <summary>Slice of unread bytes.</summary>
        public ReadOnlySpan<byte> UnreadSpan => _span[_pos..];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<byte> Take(int count)
        {
            if ((uint)count > (uint)Remaining) throw new ArgumentOutOfRangeException(nameof(count));
            var s = _span.Slice(_pos, count);
            _pos += count;
            return s;
        }

        // ---- Fixed-width primitives (LE) ----

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadU8() => Take(1)[0];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadI8() => unchecked((sbyte)ReadU8());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBool() => ReadU8() != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadU16() => BinaryPrimitives.ReadUInt16LittleEndian(Take(2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadI16() => BinaryPrimitives.ReadInt16LittleEndian(Take(2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadU32() => BinaryPrimitives.ReadUInt32LittleEndian(Take(4));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadI32() => BinaryPrimitives.ReadInt32LittleEndian(Take(4));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadU64() => BinaryPrimitives.ReadUInt64LittleEndian(Take(8));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadI64() => BinaryPrimitives.ReadInt64LittleEndian(Take(8));

        /// <summary>Reads IEEE754 single in LE without allocations.</summary>
        public float ReadF32()
        {
            var bs = Take(4);
            Span<byte> tmp = stackalloc byte[4];
            bs.CopyTo(tmp);
            if (!BitConverter.IsLittleEndian) tmp.Reverse();
            return MemoryMarshal.Read<float>(tmp);
        }

        /// <summary>Reads IEEE754 double in LE without allocations.</summary>
        public double ReadF64()
        {
            var bs = Take(8);
            Span<byte> tmp = stackalloc byte[8];
            bs.CopyTo(tmp);
            if (!BitConverter.IsLittleEndian) tmp.Reverse();
            return MemoryMarshal.Read<double>(tmp);
        }

        /// <summary>Skips a number of bytes.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip(int len) => _pos = checked(_pos + len) <= _span.Length
            ? _pos + len
            : throw new ArgumentOutOfRangeException(nameof(len));

        /// <summary>Reads exact number of bytes as a slice.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadBytes(int len) => Take(len);

        /// <summary>Returns a slice of the remaining span without advancing the cursor.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> Slice(int len)
        {
            if ((uint)len > (uint)Remaining) throw new ArgumentOutOfRangeException(nameof(len));
            return _span.Slice(_pos, len);
        }
    }
}