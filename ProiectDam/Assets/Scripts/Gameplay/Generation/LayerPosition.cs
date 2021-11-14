using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Generation
{
    public sealed class LayerPosition
    {
        private Vector2Int _pos;

        public LayerPosition(Vector2Int pos, TileType[,] layer)
        {
            _pos = pos;
            Layer = layer;
        }

        public TileType[,] Layer { get; set; }

        public bool TryMove(Vector2Int dir)
        {
            Vector2Int end = _pos + dir;

            if (Layer[end.x, end.y].CanMove())
            {
                _pos = end;
                return true;
            }

            return false;
        }
    }
}
