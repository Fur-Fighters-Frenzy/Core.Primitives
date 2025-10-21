using System;

namespace Validosik.Core.Primitives.Types
{
    /// <summary>16-bit archetype identifier.</summary>
    public readonly struct ArchetypeId : IEquatable<ArchetypeId>
    {
        public readonly ushort Value;
        public ArchetypeId(ushort value) => Value = value;
        public override string ToString() => Value.ToString();
        public bool Equals(ArchetypeId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ArchetypeId o && Equals(o);
        public override int GetHashCode() => Value;
        public static implicit operator ushort(ArchetypeId v) => v.Value;
        public static explicit operator ArchetypeId(ushort v) => new ArchetypeId(v);
    }
}