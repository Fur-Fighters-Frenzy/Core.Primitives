using System;

namespace Validosik.Core.Primitives.Hash
{
    /// <summary>
    /// CRC-32C (Castagnoli) checksum utilities.
    /// Polynomial: 0x1EDC6F41 (reflected 0x82F63B78). Table-driven, no intrinsics.
    /// </summary>
    public static class Crc32C
    {
        // Reflected polynomial for bitwise/table algo
        private const uint PolyReflected = 0x82F63B78u;

        // 256-entry table, lazily initialized once
        private static readonly uint[] Table = CreateTable();

        /// <summary>Computes CRC-32C of the given data.</summary>
        public static uint Compute(ReadOnlySpan<byte> data)
        {
            var crc = 0xFFFF_FFFFu;
            for (var i = 0; i < data.Length; ++i)
            {
                crc = (crc >> 8) ^ Table[(crc ^ data[i]) & 0xFF];
            }

            return ~crc;
        }

        /// <summary>
        /// Incremental CRC-32C accumulator. Start with Reset() (or default ctor) and feed data via Append().
        /// </summary>
        public struct Accumulator
        {
            private uint _crc;

            /// <summary>Creates an accumulator initialized to the standard seed.</summary>
            public static Accumulator Create()
            {
                var a = new Accumulator();
                a.Reset();
                return a;
            }

            /// <summary>Resets internal state to initial seed.</summary>
            public void Reset() => _crc = 0xFFFF_FFFFu;

            /// <summary>Appends a data chunk into the checksum.</summary>
            public void Append(ReadOnlySpan<byte> data)
            {
                var crc = _crc;
                for (var i = 0; i < data.Length; ++i)
                {
                    crc = (crc >> 8) ^ Table[(crc ^ data[i]) & 0xFF];
                }

                _crc = crc;
            }

            /// <summary>Gets the final CRC-32C value (bitwise negated).</summary>
            public uint GetValue() => ~_crc;
        }

        private static uint[] CreateTable()
        {
            var t = new uint[256];
            for (uint i = 0; i < 256; ++i)
            {
                var r = i;
                for (var j = 0; j < 8; ++j)
                {
                    r = (r >> 1) ^ ((r & 1) != 0 ? PolyReflected : 0u);
                }

                t[i] = r;
            }

            return t;
        }
    }
}