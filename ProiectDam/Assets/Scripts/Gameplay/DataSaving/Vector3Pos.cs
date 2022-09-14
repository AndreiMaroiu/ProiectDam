using UnityEngine;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public struct Vector3Pos
    {
        public Vector3Pos(Vector3 vec)
            => (X, Y, Z) = (vec.x, vec.y, vec.z);

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static implicit operator Vector3 (Vector3Pos pos)
            => new Vector3(pos.X, pos.Y, pos.Z);

        public static implicit operator Vector3Pos (Vector3 vec)
            => new Vector3Pos(vec);

    }
}
