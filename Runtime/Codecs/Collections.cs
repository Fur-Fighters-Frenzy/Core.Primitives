using System;
using System.Buffers;
using Validosik.Core.Primitives.Buffers;

namespace Validosik.Core.Primitives.Codecs
{
    /// <summary>Generic collection codecs. Delegate-free for ref struct compatibility.</summary>
    public static class Collections
    {
        /// <summary>
        /// Item reader abstraction that can read one T from a SpanReader.
        /// Implement as a struct to avoid allocations.
        /// </summary>
        public interface IItemReader<out T>
        {
            T Read(ref SpanReader r);
        }

        /// <summary>Writes array with prefixed length and item writer.</summary>
        public static void WriteArray<T>(ReadOnlySpan<T> items, IBufferWriter<byte> w, LenPrefix prefix,
            Action<T, IBufferWriter<byte>> writeItem)
        {
            Length.WriteLen((uint)items.Length, w, prefix);
            for (var i = 0; i < items.Length; ++i)
            {
                writeItem(items[i], w);
            }
        }

        /// <summary>
        /// Reads array with prefixed length using a value-type reader.
        /// Avoids delegates so ref struct SpanReader is usable.
        /// </summary>
        public static T[] ReadArray<T, TReader>(ref SpanReader r, LenPrefix prefix, TReader reader)
            where TReader : struct, IItemReader<T>
        {
            var len = Length.ReadLen(ref r, prefix);
            var arr = new T[len];
            for (var i = 0; i < len; ++i)
            {
                arr[i] = reader.Read(ref r);
            }

            return arr;
        }
    }
}