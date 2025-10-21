using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Validosik.Core.Primitives.Buffers;

namespace Validosik.Core.Primitives.Codecs
{
    /// <summary>Fixed-width number encoding/decoding (Little-Endian). No heap allocations.</summary>
    public static class NumbersLE
    {
        // ---- Write ----

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteU8(byte v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(1);
            s[0] = v;
            w.Advance(1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBool(bool v, IBufferWriter<byte> w) => WriteU8(v ? (byte)1 : (byte)0, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteI8(sbyte v, IBufferWriter<byte> w) => WriteU8(unchecked((byte)v), w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteU16(ushort v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(2);
            BinaryPrimitives.WriteUInt16LittleEndian(s, v);
            w.Advance(2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteI16(short v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(2);
            BinaryPrimitives.WriteInt16LittleEndian(s, v);
            w.Advance(2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteU32(uint v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(4);
            BinaryPrimitives.WriteUInt32LittleEndian(s, v);
            w.Advance(4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteI32(int v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(4);
            BinaryPrimitives.WriteInt32LittleEndian(s, v);
            w.Advance(4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteU64(ulong v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(8);
            BinaryPrimitives.WriteUInt64LittleEndian(s, v);
            w.Advance(8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteI64(long v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(8);
            BinaryPrimitives.WriteInt64LittleEndian(s, v);
            w.Advance(8);
        }

        /// <summary>Writes IEEE754 single in LE without allocations.</summary>
        public static void WriteF32(float v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(4);
            // write native-endian then normalize to LE if needed
            MemoryMarshal.Write(s, ref v);
            if (!BitConverter.IsLittleEndian)
            {
                // reverse first 4 bytes in-place of 's'
                s[..4].Reverse();
            }

            w.Advance(4);
        }

        /// <summary>Writes IEEE754 double in LE without allocations.</summary>
        public static void WriteF64(double v, IBufferWriter<byte> w)
        {
            var s = w.GetSpan(8);
            MemoryMarshal.Write(s, ref v);
            if (!BitConverter.IsLittleEndian)
            {
                s[..8].Reverse();
            }

            w.Advance(8);
        }

        /// <summary>Writes raw bytes as-is.</summary>
        public static void WriteBytes(ReadOnlySpan<byte> src, IBufferWriter<byte> w)
        {
            src.CopyTo(w.GetSpan(src.Length));
            w.Advance(src.Length);
        }

        // ---- Read (via SpanReader for symmetry) ----

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadU8(ref SpanReader r) => r.ReadU8();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadI8(ref SpanReader r) => r.ReadI8();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBool(ref SpanReader r) => r.ReadBool();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadU16(ref SpanReader r) => r.ReadU16();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadI16(ref SpanReader r) => r.ReadI16();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadU32(ref SpanReader r) => r.ReadU32();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadI32(ref SpanReader r) => r.ReadI32();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadU64(ref SpanReader r) => r.ReadU64();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadI64(ref SpanReader r) => r.ReadI64();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadF32(ref SpanReader r) => r.ReadF32();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadF64(ref SpanReader r) => r.ReadF64();
    }
}