using UnityEngine;

namespace Core.Values
{
    [System.Serializable]
    public struct IntRange
    {
        public int start;
        public int end;

        public IntRange(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        public static IntRange operator *(IntRange range, float scalar)
            => new()
            {
                start = (range.start * scalar).RoundToInt(),
                end = (range.end * scalar).RoundToInt(),
            };

        public static implicit operator Vector2Int(IntRange range)
            => new(range.start, range.end);
    }
}
