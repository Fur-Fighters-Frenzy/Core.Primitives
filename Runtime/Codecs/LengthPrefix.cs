using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Validosik.Core.Primitives.Codecs
{
    /// <summary>Length prefix kind used for strings/arrays.</summary>
    public enum LenPrefix : byte
    {
        U8,
        U16,
        U32,
        Var
    }

    /// <summary>Length encoding helpers (write/read/estimate).</summary>
    public static class Length
    {
        /// <summary>Writes length using the given prefix scheme.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLen(uint len, IBufferWriter<byte> w, LenPrefix prefix)
        {
            switch (prefix)
            {
                case LenPrefix.U8:
                    if (len > byte.MaxValue) throw new ArgumentOutOfRangeException(nameof(len));
                    NumbersLE.WriteU8((byte)len, w);
                    break;
                case LenPrefix.U16:
                    if (len > ushort.MaxValue) throw new ArgumentOutOfRangeException(nameof(len));
                    NumbersLE.WriteU16((ushort)len, w);
                    break;
                case LenPrefix.U32:
                    NumbersLE.WriteU32(len, w);
                    break;
                case LenPrefix.Var:
                    Varint.WriteVarUInt(len, w);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(prefix));
            }
        }

        /// <summary>Reads length according to the given prefix scheme.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadLen(ref Buffers.SpanReader r, LenPrefix prefix)
        {
            switch (prefix)
            {
                case LenPrefix.U8: return r.ReadU8();
                case LenPrefix.U16: return r.ReadU16();
                case LenPrefix.U32:
                {
                    var v = r.ReadU32();
                    if (v > int.MaxValue) throw new OverflowException("Length exceeds Int32.MaxValue");
                    return (int)v;
                }
                case LenPrefix.Var:
                {
                    var v = Varint.ReadVarUInt(ref r);
                    if (v > int.MaxValue) throw new OverflowException("Length exceeds Int32.MaxValue");
                    return (int)v;
                }
                default: throw new ArgumentOutOfRangeException(nameof(prefix));
            }
        }

        /// <summary>Estimates number of bytes needed to encode the given length.</summary>
        public static int EstimateLenSize(uint len, LenPrefix prefix)
        {
            return prefix switch
            {
                LenPrefix.U8 => 1,
                LenPrefix.U16 => 2,
                LenPrefix.U32 => 4,
                LenPrefix.Var => Varint.EstimateVarUIntSize(len),
                _ => throw new ArgumentOutOfRangeException(nameof(prefix))
            };
        }

        /// <summary>Tries to peek how many bytes a VarUInt-encoded length will occupy (without consuming bytes).</summary>
        public static bool TryPeekVarLen(ReadOnlySpan<byte> data, out int byteCount)
        {
            byteCount = 0;
            var i = 0;
            while (i < data.Length)
            {
                var b = data[i++];
                byteCount++;
                if ((b & 0x80) == 0) return true; // last byte
                if (byteCount >= 10) break; // 64-bit max
            }

            return false;
        }
    }
}