using UnityEngine;
using Utilities;

namespace Gameplay.Generation
{
    public class LayersSpawner
    {
        private readonly float _cellSize;
        private readonly TileSettings _tileSettings;

        private Vector3 _offset;

        public LayersSpawner(float cellSize, TileSettings tileSettings)
        {
            _cellSize = cellSize;
            _tileSettings = tileSettings;
        }

        public void GenerateLayers(RoomBehaviour behaviour)
        {
            Layers layers = behaviour.Layers;

            for (int i = 0; i < layers.Count; i++)
            {
                SpawnLayer(layers.GetTiles(i), behaviour.GetTransform(i));
            }
        }

        private void SpawnLayer(TileType[,] layer, Transform where)
        {
            int size = layer.GetLength(0);
            // world position is rotated from matrix position
            _offset = new Vector3(-size / 2 * _cellSize, size / 2 * _cellSize);

            SpawnWalls(layer, where);
            SpawnBackground(layer, where);
            SpawnForeground(layer, where);
        }

        private void SpawnTile(int i, int j, GameObject tile, Transform where)
        {
            // world position is rotated from matrix position
            Vector3 pos = new Vector3(_cellSize * -j, _cellSize * i) - _offset;

            if (!tile.IsNull())
            {
                GameObject spawnedTile = GameObject.Instantiate(tile, where);
                spawnedTile.transform.localPosition = pos;
            }
        }

        private void SpawnTile(int i, int j, GameObject tile, 
            Transform where, TileType type, TileType condition)
        {
            if (type == condition)
            {
                SpawnTile(i, j, tile, where);
            }
        }

        private void SpawnTileConditional(int i, int j, GameObject tile, 
            Transform where, TileType type, TileType condition)
        {
            if (type != condition)
            {
                SpawnTile(i, j, tile, where);
            }
        }

        private void SpawnWalls(TileType[,] layer, Transform where)
        {
            int size = layer.GetLength(0);
            int sizeMinus = size - 1;

            for (int i = 0; i < size; i++)
            {
                SpawnTile(i, 0, _tileSettings.GetTile(TileType.Wall), where, layer[i, 0], TileType.Wall);
                SpawnTile(0, i, _tileSettings.GetTile(TileType.Wall), where, layer[0, i], TileType.Wall);
                SpawnTile(sizeMinus, i, _tileSettings.GetTile(TileType.Wall), where, layer[sizeMinus, i], TileType.Wall);
                SpawnTile(i, sizeMinus, _tileSettings.GetTile(TileType.Wall), where, layer[i, sizeMinus], TileType.Wall);
            }
        }

        private void SpawnBackground(TileType[,] layer, Transform where)
        {
            int size = layer.GetLength(0);
            int sizeMinus = size;

            for (int i = 0; i < sizeMinus; i++)
            {
                for (int j = 0; j < sizeMinus; j++)
                {
                    SpawnTileConditional(i, j, _tileSettings.GetTile(TileType.None), where, layer[i, j], TileType.Wall);
                }
            }
        }

        private void SpawnForeground(TileType[,] layer, Transform where)
        {
            // minus 2 is required cause there is a wall border and a none border
            int size = layer.GetLength(0);
            int sizeMinus = size - 2;

            for (int i = 2; i < sizeMinus; i++)
            {
                for (int j = 2; j < sizeMinus; j++)
                {
                    TileType type = layer[i, j];
                    SpawnTileConditional(i, j, _tileSettings.GetTile(type), where, type, TileType.None);
                }
            }
        }
    }
}
