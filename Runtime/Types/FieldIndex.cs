using System;

namespace Validosik.Core.Primitives.Types
{
    /// <summary>8-bit field index within an archetype.</summary>
    public readonly struct FieldIndex : IEquatable<FieldIndex>
    {
        public readonly byte Value;
        public FieldIndex(byte value) => Value = value;
        public override string ToString() => Value.ToString();
        public bool Equals(FieldIndex other) => Value == other.Value;
        public override bool Equals(object obj) => obj is FieldIndex o && Equals(o);
        public override int GetHashCode() => Value;
        public static implicit operator byte(FieldIndex v) => v.Value;
        public static implicit operator FieldIndex(byte v) => new FieldIndex(v);
    }
}