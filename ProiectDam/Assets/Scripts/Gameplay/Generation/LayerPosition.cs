using UnityEngine;

namespace Gameplay.Generation
{
    public sealed class LayerPosition
    {
        private TileType[,] _layer;

        public LayerPosition(LayerPosition other)
        {
            _layer = other.Layer;
            Position = other.Position;
        }

        public LayerPosition(Vector2Int pos, TileType[,] layer)
        {
            Position = pos;
            _layer = layer;
        }

        public TileType[,] Layer
        {
            get => _layer;
            set
            {
                TileType tile = GetTile();
                GetTile() = TileType.None;

                _layer = value;

                GetTile() = tile;
            }
        }

        private ref TileType GetTile()
            => ref _layer[Position.x, Position.y];

        public Vector2Int Position { get; private set; }

        public bool CanMove(Vector2Int dir)
        {
            Vector2Int end = Position + dir;

            if (Layer[end.x, end.y].CanMovePlayer())
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
