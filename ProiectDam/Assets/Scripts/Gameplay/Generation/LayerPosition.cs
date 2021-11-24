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
                TileType tile = GetTile();
                GetTile() = TileType.None;

                _layer = value;

                GetTile() = tile;
            }
        }

        public TileType Tile => Layer[Position.x, Position.y];

        public void Move(Vector2Int dir)
        {
            Position += dir;
        }

        public void Clear()
        {
            GetTile() = TileType.None;
        }

        private ref TileType GetTile()
            => ref _layer[Position.x, Position.y];

        public TileType GetTile(Vector2Int dir)
        {
            Vector2Int pos = Position + dir;
            return Layer[pos.x, pos.y];
        }
    }
}
