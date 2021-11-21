using UnityEngine;
using Utilities;
using Core;

namespace Gameplay.Generation
{
    public class LayersSpawner
    {
        private readonly float _cellSize;
        private readonly TileSettings _grassTiles;
        private readonly TileSettings _fireTiles;
        private readonly TileSettings _dungeonTiles;

        private Vector3 _offset;

        public LayersSpawner(float cellSize, TileSettings grassTiles, TileSettings fireTiles, TileSettings dungeonTiles)
        {
            _cellSize = cellSize;

            _grassTiles = grassTiles;
            _fireTiles = fireTiles;
            _dungeonTiles = dungeonTiles;
        }

        public void GenerateLayers(RoomBehaviour behaviour)
        {
            Layers layers = behaviour.Layers;

            for (int i = 0; i < layers.Count; i++)
            {
                SpawnLayer(layers.GetTiles(i), behaviour.GetTransform(i), layers.GetBiome(i));
            }
        }

        private void SpawnLayer(TileType[,] layer, Transform where, BiomeType biome)
        {
            int halfSize = layer.GetLength(0) / 2;
            _offset = Utils.GetVector3FromMatrixPos(halfSize, halfSize, _cellSize);

            SpawnWalls(layer, where, biome);
            SpawnBackground(layer, where, biome);
            SpawnForeground(layer, where, biome);
        }

        private void SpawnWalls(TileType[,] layer, Transform where, BiomeType biome)
        {
            int size = layer.GetLength(0);
            int sizeMinus = size - 1;

            for (int i = 0; i < size; i++)
            {
                SpawnTileIf(i, 0, where, biome, layer[i, 0], TileType.Wall);
                SpawnTileIf(0, i, where, biome, layer[0, i], TileType.Wall);

                SpawnTileIf(sizeMinus, i, where, biome, layer[sizeMinus, i], TileType.Wall);
                SpawnTileIf(i, sizeMinus, where, biome, layer[i, sizeMinus], TileType.Wall);
            }
        }

        private void SpawnBackground(TileType[,] layer, Transform where, BiomeType biome)
        {
            int size = layer.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    SpawnTileIfNot(i, j, where, biome, TileType.None, layer[i, j], TileType.Wall);
                }
            }
        }

        private void SpawnForeground(TileType[,] layer, Transform where, BiomeType biome)
        {
            // minus 2 is required because there is a wall border and a none border
            int size = layer.GetLength(0);
            int sizeMinus = size - 2;

            for (int i = 2; i < sizeMinus; i++)
            {
                for (int j = 2; j < sizeMinus; j++)
                {
                    TileType type = layer[i, j];

                    if (type is TileType.None)
                    {
                        continue;
                    }

                    SpawnTile(i, j, where, biome, type);
                }
            }
        }

        #region Helpers

        private void SpawnTile(int i, int j, Transform where, BiomeType biome, TileType type)
        {
            Vector3 pos = Utils.GetVector3FromMatrixPos(i, j, _cellSize) - _offset;
            GameObject tile = GetSettings(biome).GetTile(type);

            if (tile.IsNotNull())
            {
                GameObject spawnedTile = GameObject.Instantiate(tile, where);
                spawnedTile.transform.localPosition = pos;
            }
        }

        private void SpawnTileIf(int i, int j, Transform where, BiomeType biome, TileType type, TileType condition)
        {
            if (type == condition)
            {
                SpawnTile(i, j, where, biome, type);
            }
        }

        private void SpawnTileIfNot(int i, int j, Transform where, BiomeType biome,
            TileType type, TileType tile, TileType condition)
        {
            if (tile != condition)
            {
                SpawnTile(i, j, where, biome, type);
            }
        }

        private TileSettings GetSettings(BiomeType biome) => biome switch
        {
            BiomeType.None => null,
            BiomeType.Fire => _fireTiles,
            BiomeType.Dungeon => _dungeonTiles,
            BiomeType.Grassland => _grassTiles,
            _ => null,
        };

        #endregion
    }
}
