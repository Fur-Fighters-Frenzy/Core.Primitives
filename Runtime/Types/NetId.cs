using System;

namespace Validosik.Core.Primitives.Types
{
    /// <summary>
    /// 32-bit network entity identifier, packed as:
    /// [ Gen12 (bits 31..20) | Id20 (bits 19..0) ].
    /// Generation is 12-bit and wraps on overflow.
    /// </summary>
    public readonly struct NetId : IEquatable<NetId>
    {
        public readonly uint Value;
        public NetId(uint value) => Value = value;

        // Packing constants
        public const int GenShift = 20;
        public const uint IdMask = 0x000F_FFFFu; // 20 bits
        public const uint GenMask = 0xFFF0_0000u; // 12 bits (shifted)

        /// <summary>Returns the 20-bit entity id part (0..1_048_575).</summary>
        public uint Id20 => Value & IdMask;

        /// <summary>Returns the 12-bit generation part (0..4095).</summary>
        public ushort Gen12 => (ushort)((Value & GenMask) >> GenShift);

        /// <summary>Creates a packed NetId from 20-bit id and 12-bit generation.</summary>
        public static NetId Compose(uint id20, ushort gen12)
        {
            var v = (id20 & IdMask) | (((uint)gen12 & 0x0FFFu) << GenShift);
            return new NetId(v);
        }

        /// <summary>Returns a copy with the id part replaced.</summary>
        public NetId WithId(uint id20) => Compose(id20, Gen12);

        /// <summary>Returns a copy with the generation replaced (masked to 12 bits).</summary>
        public NetId WithGen(ushort gen12) => Compose(Id20, gen12);

        /// <summary>Returns a copy with generation incremented and wrapped to 12 bits.</summary>
        public NetId NextGen() => Compose(Id20, (ushort)((Gen12 + 1) & 0x0FFF));

        /// <summary>Decomposes into (id20, gen12).</summary>
        public void Decompose(out uint id20, out ushort gen12)
        {
            id20 = Id20;
            gen12 = Gen12;
        }

        public override string ToString() => $"{Id20}:{Gen12}";
        public bool Equals(NetId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is NetId o && Equals(o);
        public override int GetHashCode() => (int)Value;

        public static implicit operator uint(NetId v) => v.Value;
        public static explicit operator NetId(uint v) => new NetId(v);
    }
}