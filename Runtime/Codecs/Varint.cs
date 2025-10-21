using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using Validosik.Core.Primitives.Buffers;

namespace Validosik.Core.Primitives.Codecs
{
    /// <summary>LEB128-style variable-length integer encoding: VarUInt and ZigZag VarInt.</summary>
    public static class Varint
    {
        /// <summary>Writes unsigned integer using base-128 VarUInt (7 bits per byte, MSB=continuation).</summary>
        public static void WriteVarUInt(ulong v, IBufferWriter<byte> w)
        {
            while (v >= 0x80)
            {
                var s = w.GetSpan(1);
                s[0] = (byte)(0x80 | (v & 0x7Fu));
                w.Advance(1);
                v >>= 7;
            }

            var last = w.GetSpan(1);
            last[0] = (byte)v;
            w.Advance(1);
        }

        /// <summary>Reads unsigned VarUInt encoded value.</summary>
        public static ulong ReadVarUInt(ref SpanReader r)
        {
            ulong result = 0;
            var shift = 0;
            while (true)
            {
                var b = r.ReadU8();
                result |= (ulong)(b & 0x7Fu) << shift;
                if ((b & 0x80u) == 0) break;
                shift += 7;
                if (shift >= 64) throw new FormatException("VarUInt too long");
            }

            return result;
        }

        /// <summary>Returns number of bytes VarUInt would occupy.</summary>
        public static int EstimateVarUIntSize(ulong v)
        {
            var count = 1;
            while (v >= 0x80)
            {
                v >>= 7;
                count++;
            }

            return count;
        }

        /// <summary>Writes signed integer using ZigZag+VarUInt (maps small negatives to small positives).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVarInt(long v, IBufferWriter<byte> w)
            => WriteVarUInt(EncodeZigZag(v), w);

        /// <summary>Reads signed ZigZag+VarUInt encoded value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadVarInt(ref SpanReader r)
            => DecodeZigZag(ReadVarUInt(ref r));

        /// <summary>Encodes signed value to unsigned via ZigZag: 0→0, -1→1, 1→2, -2→3, …</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong EncodeZigZag(long v) => (ulong)((v << 1) ^ (v >> 63));

        /// <summary>Decodes ZigZag back to signed value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long DecodeZigZag(ulong u) => (long)(u >> 1) ^ -((long)(u & 1));
    }
}