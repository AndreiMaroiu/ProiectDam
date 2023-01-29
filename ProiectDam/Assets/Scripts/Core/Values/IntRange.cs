using UnityEngine;

namespace Core.Values
{
    [System.Serializable]
    public struct IntRange
    {
        public int start;
        public int end;

        public static IntRange operator *(IntRange range, float scalar)
            => new()
            {
                start = (int)Mathf.Round(range.start * scalar),
                end = (int)Mathf.Round(range.end * scalar),
            };

        public static implicit operator Vector2Int(IntRange range)
            => new(range.start, range.end);
    }
}
