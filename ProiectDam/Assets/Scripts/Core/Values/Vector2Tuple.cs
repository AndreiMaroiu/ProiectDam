using System;

namespace Core.Values
{
    public readonly struct Vector2Tuple
    {
        public readonly Vector2IntPos pos1;
        public readonly Vector2IntPos pos2;

        public Vector2Tuple(Vector2IntPos pos1, Vector2IntPos pos2)
        {
            this.pos1 = pos1;
            this.pos2 = pos2;
        }

        public override readonly int GetHashCode()
        {
            if (pos1.SqrMagnitude < pos2.SqrMagnitude)
            {
                return HashCode.Combine(pos1.GetHashCode(), pos2.GetHashCode());
            }
            else
            {
                return HashCode.Combine(pos2.GetHashCode(), pos1.GetHashCode());
            }
        }
    }
}
