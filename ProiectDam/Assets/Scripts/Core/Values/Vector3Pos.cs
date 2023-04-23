using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Core.Values
{
    [Serializable]
    public struct Vector3Pos
    {
        public Vector3Pos(Vector3 vec)
            => (X, Y, Z) = (vec.x, vec.y, vec.z);

        public float X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public float Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public float Z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public static implicit operator Vector3(Vector3Pos pos)
            => new(pos.X, pos.Y, pos.Z);

        public static implicit operator Vector3Pos(Vector3 vec)
            => new(vec);

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

    }
}
