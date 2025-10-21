using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using Validosik.Core.Primitives.Buffers;

namespace Validosik.Core.Primitives.Codecs
{
    /// <summary>UTF-8 encoding with configurable length prefix.</summary>
    public static class Text
    {
        /// <summary>Writes UTF-8 string with length prefix. Null is encoded as zero-length.</summary>
        public static void WriteUtf8(string s, IBufferWriter<byte> w, LenPrefix prefix)
        {
            if (s == null)
            {
                Length.WriteLen(0, w, prefix);
                return;
            }

            var byteCount = Encoding.UTF8.GetByteCount(s);
            Length.WriteLen((uint)byteCount, w, prefix);
            var span = w.GetSpan(byteCount);
            _ = Encoding.UTF8.GetBytes(s, span);
            w.Advance(byteCount);
        }

        /// <summary>Reads UTF-8 string with given length prefix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadUtf8(ref SpanReader r, LenPrefix prefix)
        {
            var len = Length.ReadLen(ref r, prefix);
            if (len == 0)
            {
                return string.Empty;
            }

            var bytes = r.ReadBytes(len);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}