using UnityEngine;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public struct Vector2IntPos
    {
        //public Vector2IntPos() { }

        public Vector2IntPos(Vector2Int vec)
            => (X, Y) = (vec.x, vec.y);

        public int X { get; set; }
        public int Y { get; set; }

        public static implicit operator Vector2Int (Vector2IntPos pos)
            => new Vector2Int(pos.X, pos.Y);

        public static implicit operator Vector2IntPos (Vector2Int vec)
            => new Vector2IntPos(vec);
    }
}