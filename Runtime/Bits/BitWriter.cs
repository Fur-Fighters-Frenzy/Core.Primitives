using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using Validosik.Core.Primitives.Codecs;

namespace Validosik.Core.Primitives.Bits
{
    /// <summary>Packs bits into bytes on top of IBufferWriter&lt;byte&gt; (LSB-first within a byte).</summary>
    public ref struct BitWriter
    {
        private readonly IBufferWriter<byte> _w;
        private byte _acc;
        private int _bits; // occupied bits in _acc [0..7]

        public BitWriter(IBufferWriter<byte> writer)
        {
            _w = writer;
            _acc = 0;
            _bits = 0;
        }

        /// <summary>Writes exactly bitCount bits from value (lower bits first).</summary>
        public void WriteBits(uint value, int bitCount)
        {
            if ((uint)bitCount > 32u)
            {
                throw new ArgumentOutOfRangeException(nameof(bitCount));
            }

            var v = value;
            var remaining = bitCount;

            while (remaining > 0)
            {
                var can = 8 - _bits;
                var take = remaining < can ? remaining : can;
                var chunk = (byte)((v & ((1u << take) - 1)) << _bits);
                _acc |= chunk;
                _bits += take;
                v >>= take;
                remaining -= take;

                if (_bits != 8)
                {
                    continue;
                }

                NumbersLE.WriteU8(_acc, _w);
                _acc = 0;
                _bits = 0;
            }
        }

        /// <summary>Writes a single bit as boolean.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBool(bool v) => WriteBits(v ? 1u : 0u, 1);

        /// <summary>Flushes the partially filled byte (if any).</summary>
        public void Flush()
        {
            if (_bits <= 0)
            {
                return;
            }

            NumbersLE.WriteU8(_acc, _w);
            _acc = 0;
            _bits = 0;
        }
    }
}