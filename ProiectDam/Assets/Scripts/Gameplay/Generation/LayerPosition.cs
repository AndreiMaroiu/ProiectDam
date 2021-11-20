using UnityEngine;

namespace Gameplay.Generation
{
    public sealed class LayerPosition
    {
        public LayerPosition(LayerPosition other)
        {
            Layer = other.Layer;
            Position = other.Position;
        }

        public LayerPosition(Vector2Int pos, TileType[,] layer)
        {
            Position = pos;
            Layer = layer;
        }

        public TileType[,] Layer
        {
            get;
            set;
        }

        public Vector2Int Position { get; private set; }

        public bool CanMove(Vector2Int dir)
        {
            Vector2Int end = Position + dir;

            if (Layer[end.x, end.y].CanMove())
            {
                return true;
            }

            return false;
        }

        public void Move(Vector2Int dir)
        {
            Position += dir;
        }
    }
}
