using System;
using System.Runtime.CompilerServices;

namespace Validosik.Core.Primitives
{
    /// <summary>32-bit network entity identifier.</summary>
    public readonly struct NetId : IEquatable<NetId>
    {
        public readonly uint Value;
        public NetId(uint value) => Value = value;
        public override string ToString() => Value.ToString();
        public bool Equals(NetId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is NetId o && Equals(o);
        public override int GetHashCode() => (int)Value;
        public static implicit operator uint(NetId v) => v.Value;
        public static explicit operator NetId(uint v) => new NetId(v);
    }

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
        public static explicit operator FieldIndex(byte v) => new FieldIndex(v);
    }

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

    /// <summary>Server UTC time in Unix epoch milliseconds.</summary>
    public readonly struct ServerTime : IEquatable<ServerTime>
    {
        public readonly long EpochMs;
        public ServerTime(long epochMs) => EpochMs = epochMs;
        public DateTime ToDateTimeUtc() => DateTimeOffset.FromUnixTimeMilliseconds(EpochMs).UtcDateTime;

        public static ServerTime FromDateTimeUtc(DateTime utc) =>
            new ServerTime(new DateTimeOffset(utc, TimeSpan.Zero).ToUnixTimeMilliseconds());

        public override string ToString() => EpochMs.ToString();
        public bool Equals(ServerTime other) => EpochMs == other.EpochMs;
        public override bool Equals(object obj) => obj is ServerTime o && Equals(o);
        public override int GetHashCode() => EpochMs.GetHashCode();
    }

    /// <summary>Short factory helpers for ID value wrappers.</summary>
    public static class Ids
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NetId Net(uint v) => new NetId(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArchetypeId Arch(ushort v) => new ArchetypeId(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldIndex Field(byte v) => new FieldIndex(v);
    }
}