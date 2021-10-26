using UnityEngine;
using Utilities;

namespace Gameplay.Generation
{
    public class LayersSpawner
    {
        private readonly float _cellSize;
        private readonly TileSettings _tileSettings;

        public LayersSpawner(float cellSize, TileSettings tileSettings)
        {
            _cellSize = cellSize;
            _tileSettings = tileSettings;
        }

        public void GenerateLayers(RoomBehaviour behaviour)
        {
            Spawn(behaviour);
        }

        private void Spawn(RoomBehaviour room)
        {
            Layers layers = room.Layers;

            for (int i = 0; i < layers.Count; i++)
            {
                SpawnLayer(layers.GetTiles(i), room.GetTransform(i));
            }
        }

        private void SpawnLayer(TileType[,] layer, Transform where)
        {
            int size = layer.GetLength(0);
            Vector3 offset = new Vector3(-size / 2 * _cellSize, size / 2 * _cellSize);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // world position is rotated from matrix position
                    Vector3 pos = new Vector3(_cellSize * -j, _cellSize * i) - offset;
                    GameObject tile = _tileSettings.GetTile(layer[i, j]);

                    if (!tile.IsNull())
                    {
                        GameObject spawnedTile = GameObject.Instantiate(tile, where);
                        spawnedTile.transform.localPosition = pos;
                    }
                }
            }
        }
    }
}
