using UnityEngine;

namespace Gameplay
{
    internal static class Utils
    {
        public static Vector2Int GetDirectionRounded(float angle)
        {
            angle *= Mathf.Deg2Rad;
            return new Vector2Int(Mathf.RoundToInt(Mathf.Cos(angle)), Mathf.RoundToInt(Mathf.Sin(angle)));
        }

        public static Vector2 GetDirection(float angle)
        {
            angle *= Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public static Vector3 GetWorldDirection(float angle)
        {
            angle -= 90;
            return GetDirection(angle);
        }

        public static Vector3 GetVector3FromMatrixPos(int i, int j, float cellSize = 1.0f)
        {
            return new Vector3(cellSize * -j, cellSize * i);
        }

        public static Vector3 GetVector3FromMatrixPos(Vector2Int pos, float cellSize = 1.0f)
        {
            return new Vector3(cellSize * -pos.y, cellSize * pos.x);
        }
    }
}
