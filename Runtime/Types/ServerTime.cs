using System;

namespace Validosik.Core.Primitives.Types
{
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
}