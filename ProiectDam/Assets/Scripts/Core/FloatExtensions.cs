using UnityEngine;

namespace Core
{
    public static class FloatExtensions
    {
        public static int RoundToInt(this float f) => Mathf.RoundToInt(f);
    }
}
