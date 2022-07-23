using UnityEngine;

namespace Gameplay.Generation
{
    public sealed class LayerPosition
    {
        private TileType[,] _layer;

        public LayerPosition(LayerPosition other) 
            : this(other.Position, other.Layer) { }

        public LayerPosition(Vector2Int pos, TileType[,] layer)
        {
            Position = pos;
            _layer = layer;
        }

        public Vector2Int Position { get; private set; }

        public TileType[,] Layer
        {
            get => _layer;
            set
            {
                TileType tile = TileType;
                TileType = TileType.None;

                _layer = value;

                TileType = tile;
            }
        }

        public TileType TileType 
        {
            get => Layer[Position.x, Position.y];
            set => Layer[Position.x, Position.y] = value;
        }

        public void Move(Vector2Int dir) => Position += dir;

        public void Clear() => TileType = TileType.None;

        public TileType GetTile(Vector2Int dir)
        {
            Vector2Int pos = Position + dir;
            return Layer[pos.x, pos.y];
        }
    }
}
