using System.Collections;
using System.Collections.Generic;
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

            try
            {
                if (Layer[end.x, end.y].CanMove())
                {
                    return true;
                }
            }
            catch
            {
                Debug.Log($"Could not move, pos: {Position.ToString()}, end: {end.ToString()}");
            }

            return false;
        }

        public void Move(Vector2Int dir)
        {
            Position += dir;
        }
    }
}
