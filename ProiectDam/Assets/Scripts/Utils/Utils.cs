using UnityEngine;

namespace Utilities
{
    public static class Utils
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
            return new Vector3(cellSize * j, cellSize * -i);
        }

        public static Vector3 GetVector3FromMatrixPos(Vector2Int pos, float cellSize = 1.0f)
        {
            return GetVector3FromMatrixPos(pos.x, pos.y, cellSize);
        }

        public static Vector2Int GetMatrixPos(Vector2Int dir)
        {
            return new Vector2Int(-dir.y, dir.x);
        }
    }
}
