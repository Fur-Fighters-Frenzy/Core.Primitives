using System.Runtime.CompilerServices;

namespace Validosik.Core.Primitives.Types
{
    /// <summary>
    /// Pool slot metadata for an entity store.
    /// Tracks occupancy, archetype, and 12-bit local generation used to validate NetId.
    /// </summary>
    public struct Slot
    {
        /// <summary>True if a live entity currently occupies this slot.</summary>
        public bool Occupied;

        /// <summary>Optional archetype tag for fast lookups/filters.</summary>
        public ArchetypeId Archetype;

        // Local 12-bit generation (0..4095). Stored separately; compared with NetId.Gen12.
        private ushort _gen12;

        /// <summary>Current 12-bit generation value (masked).</summary>
        public ushort Generation12 => (ushort)(_gen12 & 0x0FFF);

        /// <summary>Creates an empty slot with generation = 0.</summary>
        public static Slot CreateEmpty() => new Slot
        {
            Occupied = false,
            Archetype = default,
            _gen12 = 0
        };

        /// <summary>Resets the slot (occupied=false, archetype=default, gen=0).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Occupied = false;
            Archetype = default;
            _gen12 = 0;
        }

        /// <summary>Returns true if slot is occupied and NetId's generation matches current generation.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(NetId netId) => Occupied && netId.Gen12 == Generation12;

        /// <summary>
        /// Attaches an externally provided NetId (e.g., from server) to this slot.
        /// Copies Gen12 from the NetId and marks the slot occupied with the given archetype.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Attach(NetId netId, ArchetypeId archetype)
        {
            _gen12 = (ushort)(netId.Gen12 & 0x0FFF);
            Archetype = archetype;
            Occupied = true;
        }

        /// <summary>
        /// Allocates the slot for a new entity. Returns the NetId composed from provided 20-bit id and current 12-bit generation.
        /// Caller is responsible for ensuring 'id20' fits 20 bits.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NetId Allocate(uint id20, ArchetypeId archetype)
        {
            Occupied = true;
            Archetype = archetype;
            return NetId.Compose(id20, Generation12);
        }

        /// <summary>
        /// Frees the slot: clears occupancy/archetype and bumps the 12-bit generation (wraps at 4096).
        /// This invalidates any stale NetId targeting the old generation of this slot.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Free()
        {
            Occupied = false;
            Archetype = default;
            _gen12 = (ushort)((_gen12 + 1) & 0x0FFF);
        }
    }
}