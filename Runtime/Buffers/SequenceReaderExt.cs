using System.Buffers;

namespace Validosik.Core.Primitives.Buffers
{
    /// <summary>Helpers for fragmented input via SequenceReader&lt;byte&gt; (LE). Bridges to signed overloads.</summary>
    public static class SequenceReaderExt
    {
        public static bool TryReadU8(this ref SequenceReader<byte> r, out byte v)
            => r.TryRead(out v);

        public static bool TryReadU16(this ref SequenceReader<byte> r, out ushort v)
        {
            if (r.TryReadLittleEndian(out short s))
            {
                v = unchecked((ushort)s);
                return true;
            }

            v = default;
            return false;
        }

        public static bool TryReadU32(this ref SequenceReader<byte> r, out uint v)
        {
            if (r.TryReadLittleEndian(out int s))
            {
                v = unchecked((uint)s);
                return true;
            }

            v = default;
            return false;
        }

        public static bool TryReadU64(this ref SequenceReader<byte> r, out ulong v)
        {
            if (r.TryReadLittleEndian(out long s))
            {
                v = unchecked((ulong)s);
                return true;
            }

            v = default;
            return false;
        }

        public static bool TryReadI16(this ref SequenceReader<byte> r, out short v)
            => r.TryReadLittleEndian(out v);

        public static bool TryReadI32(this ref SequenceReader<byte> r, out int v)
            => r.TryReadLittleEndian(out v);

        public static bool TryReadI64(this ref SequenceReader<byte> r, out long v)
            => r.TryReadLittleEndian(out v);
    }
}