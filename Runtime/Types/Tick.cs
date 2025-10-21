using System;

namespace Validosik.Core.Primitives.Types
{
    /// <summary>Unsigned simulation tick counter.</summary>
    public readonly struct Tick : IEquatable<Tick>
    {
        public readonly uint Value;
        public Tick(uint value) => Value = value;
        public override string ToString() => Value.ToString();
        public bool Equals(Tick other) => Value == other.Value;
        public override bool Equals(object obj) => obj is Tick o && Equals(o);
        public override int GetHashCode() => (int)Value;
        public static implicit operator uint(Tick v) => v.Value;
        public static explicit operator Tick(uint v) => new Tick(v);
        public static Tick Zero => new Tick(0);
    }
}