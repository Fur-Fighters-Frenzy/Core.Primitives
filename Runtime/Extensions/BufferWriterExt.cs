using System;
using System.Buffers;
using Validosik.Core.Primitives.Codecs;

namespace Validosik.Core.Primitives.Extensions
{
    /// <summary>Convenience extension methods for IBufferWriter&lt;byte&gt;.</summary>
    public static class BufferWriterExt
    {
        public static void Write(this IBufferWriter<byte> w, ReadOnlySpan<byte> src) => NumbersLE.WriteBytes(src, w);

        // Fixed-width
        public static void WriteU8(this IBufferWriter<byte> w, byte v) => NumbersLE.WriteU8(v, w);
        public static void WriteI8(this IBufferWriter<byte> w, sbyte v) => NumbersLE.WriteI8(v, w);
        public static void WriteBool(this IBufferWriter<byte> w, bool v) => NumbersLE.WriteBool(v, w);
        public static void WriteU16(this IBufferWriter<byte> w, ushort v) => NumbersLE.WriteU16(v, w);
        public static void WriteI16(this IBufferWriter<byte> w, short v) => NumbersLE.WriteI16(v, w);
        public static void WriteU32(this IBufferWriter<byte> w, uint v) => NumbersLE.WriteU32(v, w);
        public static void WriteI32(this IBufferWriter<byte> w, int v) => NumbersLE.WriteI32(v, w);
        public static void WriteU64(this IBufferWriter<byte> w, ulong v) => NumbersLE.WriteU64(v, w);
        public static void WriteI64(this IBufferWriter<byte> w, long v) => NumbersLE.WriteI64(v, w);
        public static void WriteF32(this IBufferWriter<byte> w, float v) => NumbersLE.WriteF32(v, w);
        public static void WriteF64(this IBufferWriter<byte> w, double v) => NumbersLE.WriteF64(v, w);

        // Varints
        public static void WriteVarUInt(this IBufferWriter<byte> w, ulong v) => Varint.WriteVarUInt(v, w);
        public static void WriteVarInt(this IBufferWriter<byte> w, long v) => Varint.WriteVarInt(v, w);

        // Text & bytes with length
        public static void WriteUtf8(this IBufferWriter<byte> w, string s, LenPrefix pfx) => Text.WriteUtf8(s, w, pfx);

        public static void WriteWithLen(this IBufferWriter<byte> w, ReadOnlySpan<byte> src, LenPrefix pfx)
        {
            Length.WriteLen((uint)src.Length, w, pfx);
            NumbersLE.WriteBytes(src, w);
        }
    }
}