using System.Runtime.CompilerServices;

namespace Validosik.Core.Primitives.Types
{
    /// <summary>Short factory helpers for ID value wrappers.</summary>
    public static class Ids
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NetId Net(uint v) => new NetId(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArchetypeId Archetype(ushort v) => new ArchetypeId(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComponentId Component(ushort v) => new ComponentId(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldIndex Field(byte v) => new FieldIndex(v);
    }
}