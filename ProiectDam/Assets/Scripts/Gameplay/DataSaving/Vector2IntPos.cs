using System;
using UnityEngine;

namespace Gameplay.DataSaving
{
    [Serializable]
    public struct Vector2IntPos
    {
        public Vector2IntPos(Vector2Int vec)
            => (X, Y) = (vec.x, vec.y);

        public Vector2IntPos(int x, int y)
            => (X, Y) = (x, y);

        public int X { get; set; }
        public int Y { get; set; }

        public static implicit operator Vector2Int (Vector2IntPos pos)
            => new Vector2Int(pos.X, pos.Y);

        public static implicit operator Vector2IntPos (Vector2Int vec)
            => new Vector2IntPos(vec);

        public static bool operator ==(Vector2IntPos left, Vector2IntPos right)
            => left.X == right.X && left.Y == right.Y;

        public static bool operator !=(Vector2IntPos left, Vector2IntPos right)
            => left.X != right.X || left.Y != right.Y;

        public override bool Equals(object obj)
        {
            if (obj is Vector2IntPos vector)
            {
                return this == vector;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
        }
    }
}