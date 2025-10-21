using Validosik.Core.Primitives.Buffers;
using Validosik.Core.Primitives.Codecs;

namespace Validosik.Core.Primitives.Extensions
{
    /// <summary>Convenience extensions mirroring SpanReader/Codecs APIs.</summary>
    public static class SpanReaderExt
    {
        public static ulong ReadVarUInt(ref this SpanReader r) => Varint.ReadVarUInt(ref r);
        public static long ReadVarInt(ref this SpanReader r) => Varint.ReadVarInt(ref r);
        public static string ReadUtf8(ref this SpanReader r, LenPrefix prefix) => Text.ReadUtf8(ref r, prefix);
    }
}