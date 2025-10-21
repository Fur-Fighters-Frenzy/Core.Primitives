using System;
using System.Runtime.CompilerServices;
using Validosik.Core.Primitives.Buffers;

namespace Validosik.Core.Primitives.Bits
{
    /// <summary>Reads packed bits from a byte stream (LSB-first within a byte).</summary>
    public ref struct BitReader
    {
        private SpanReader _r;
        private byte _acc;
        private int _bits; // available bits in _acc

        public BitReader(ReadOnlySpan<byte> bytes)
        {
            _r = new SpanReader(bytes);
            _acc = 0;
            _bits = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureByte()
        {
            if (_bits != 0)
            {
                return;
            }

            _acc = _r.ReadU8();
            _bits = 8;
        }

        /// <summary>Reads exactly bitCount bits and returns them in the low bits of the result.</summary>
        public uint ReadBits(int bitCount)
        {
            if ((uint)bitCount > 32u) throw new ArgumentOutOfRangeException(nameof(bitCount));
            uint result = 0;
            var filled = 0;

            while (filled < bitCount)
            {
                EnsureByte();
                var take = Math.Min(_bits, bitCount - filled);
                var part = (uint)(_acc & ((1 << take) - 1));
                result |= part << filled;
                _acc >>= take;
                _bits -= take;
                filled += take;
            }

            return result;
        }

        /// <summary>Reads a single bit as boolean.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBool() => ReadBits(1) != 0;

        /// <summary>Returns unread tail of the underlying byte stream (byte-aligned).</summary>
        public ReadOnlySpan<byte> RemainingBytes => _r.UnreadSpan;
    }
}