using System;
using System.Runtime.CompilerServices;

namespace Validosik.Core.Primitives.Errors
{
    /// <summary>Tiny guard/throw helpers to keep call sites clean.</summary>
    public static class Guard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void True(bool condition, string message = "Guard.True failed")
        {
            if (!condition) throw new InvalidOperationException(message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Range(bool condition, string message = "Guard.Range failed")
        {
            if (!condition) throw new ArgumentOutOfRangeException(message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NotNull<T>(T value, string message = "Guard.NotNull failed") where T : class
            => value ?? throw new ArgumentNullException(message);
    }
}