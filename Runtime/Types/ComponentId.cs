using System;

namespace Validosik.Core.Primitives.Types
{
    /// <summary>
    /// Component type identifier (stable across runtime; used to map component schemas/slots).
    /// Backed by ushort to keep on-wire size small.
    /// </summary>
    public readonly struct ComponentId : IEquatable<ComponentId>
    {
        public readonly ushort Value;
        public ComponentId(ushort value) => Value = value;

        public override string ToString() => Value.ToString();
        public bool Equals(ComponentId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ComponentId o && Equals(o);
        public override int GetHashCode() => Value;

        public static implicit operator ushort(ComponentId v) => v.Value;
        public static explicit operator ComponentId(ushort v) => new ComponentId(v);

        public static ComponentId None => new ComponentId(0);
    }
}