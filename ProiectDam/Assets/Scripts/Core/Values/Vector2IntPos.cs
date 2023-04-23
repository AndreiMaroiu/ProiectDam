using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Core.Values
{
    [Serializable]
    public struct Vector2IntPos
    {
        public Vector2IntPos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public int Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public static Vector2IntPos operator +(Vector2IntPos left, Vector2IntPos right)
            => new() { X = left.X + right.X, Y = left.Y + right.Y };

        public static Vector2IntPos operator +(Vector2IntPos left, Vector2Int right)
            => new() { X = left.X + right.x, Y = left.Y + right.y };

        public static Vector2IntPos operator -(Vector2IntPos left, Vector2IntPos right)
            => new() { X = left.X - right.X, Y = left.Y - right.Y };

        public static Vector2IntPos operator -(Vector2IntPos left, Vector2Int right)
            => new() { X = left.X - right.x, Y = left.Y - right.y };

        public static Vector2IntPos operator -(Vector2Int left, Vector2IntPos right)
            => new() { X = left.x - right.X, Y = left.y - right.Y };

        public static implicit operator Vector2IntPos (Vector2Int vector2)
            => new() { X =  vector2.x, Y = vector2.y };

        public static implicit operator Vector2Int(Vector2IntPos pos)
            => new() { x = pos.X, y = pos.Y };

        public static implicit operator Vector2(Vector2IntPos pos)
            => new() { x = pos.X, y = pos.Y };

        public static bool operator == (Vector2IntPos left, Vector2IntPos right)
            => left.X == right.X && left.Y == right.Y;

        public static bool operator !=(Vector2IntPos left, Vector2IntPos right)
            => left.X != right.X || left.Y != right.Y;

        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        public readonly int SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => X * X + Y * Y;
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is Vector2IntPos other)
            {
                return this == other;
            }

            return false;
        }

        public override readonly string ToString() => $"X: {X.ToString()} Y: {Y.ToString()}";
    }
}
