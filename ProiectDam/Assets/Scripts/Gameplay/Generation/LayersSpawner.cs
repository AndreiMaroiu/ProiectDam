using Gameplay.DataSaving;
using System;
using UnityEngine;
using Utilities;

namespace Gameplay.Generation
{
    public class LayersSpawner
    {
        public delegate GameObject TileGeneration(TileType tileType, BiomeType biomeType, int x, int y);

        public delegate void AfterTileSpawnedAction(TileObject tile, BiomeType biomeType, int x, int y);

        public enum GenerationStrategy
        {
            Random,
            FromSave,
        }

        private readonly float _cellSize;
        private readonly TileSettings _grassTiles;
        private readonly TileSettings _fireTiles;
        private readonly TileSettings _dungeonTiles;

        private Vector3 _offset;
        private TileGeneration _generationStrategy;
        private AfterTileSpawnedAction _afterTileSpawnedAction;

        public LayersSpawner(float cellSize, TileSettings grassTiles, TileSettings fireTiles, TileSettings dungeonTiles)
        {
            _cellSize = cellSize;

            _grassTiles = grassTiles;
            _fireTiles = fireTiles;
            _dungeonTiles = dungeonTiles;

            _generationStrategy = RandomGeneration;
        }

        public LayersSaveData SaveData { get; set; }

        /// <summary>
        /// Set different tile generation strategy. By default it's random
        /// </summary>
        public void SetGenerationStrategy(GenerationStrategy strategy)
        {
            switch (strategy)
            {
                case GenerationStrategy.Random:
                    _generationStrategy = RandomGeneration;
                    _afterTileSpawnedAction = null;
                    break;
                case GenerationStrategy.FromSave:
                    _generationStrategy = GenerateFromSave;
                    _afterTileSpawnedAction = AfterTileLoaded;
                    break;
                default:
                    break;
            }
        }

        public void SpawnStatic(RoomBehaviour behaviour)
        {
            Layers layers = behaviour.Layers;

            for (int i = 0; i < layers.Count; i++)
            {
                SpawnStaticLayer(layers.GetTiles(i), behaviour.GetTransform(i), layers.GetBiome(i));
            }
        }

        public void SpawnDynamic(RoomBehaviour behaviour)
        {
            Layers layers = behaviour.Layers;

            for (int i = 0; i < layers.Count; i++)
            {
                SpawnDynamicLayer(layers.GetTiles(i), behaviour.GetTransform(i), layers.GetBiome(i));
            }
        }

        private void SpawnStaticLayer(TileType[,] layer, Transform where, BiomeType biome)
        {
            SetOffset(layer);

            SpawnBackground(layer, where, biome);
            SpawnWalls(layer, where, biome);
            SpawnForegroundIf(layer, where, biome, type => type.IsStatic() && type != TileType.None);
        }

        private void SpawnDynamicLayer(TileType[,] layer, Transform where, BiomeType biome)
        {
            SetOffset(layer);

            SpawnForegroundIf(layer, where, biome, type => !type.IsStatic());
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
                    TileType tile = layer[i, j];

                    if (tile == TileType.Wall)
                    {
                        continue;
                    }

                    SpawnTile(i, j, where, biome, TileType.None);
                }
            }
        }

        private void SpawnForegroundIf(TileType[,] layer, Transform where, BiomeType biome, Func<TileType, bool> condition)
        {
            // minus 1 is required because there is a wall border
            int size = layer.GetLength(0);
            int sizeMinus = size - 1;

            for (int i = 1; i < sizeMinus; i++)
            {
                for (int j = 1; j < sizeMinus; j++)
                {
                    TileType type = layer[i, j];

                    if (!condition(type))
                    {
                        continue;
                    }

                    SpawnTile(i, j, where, biome, type, layer);
                }
            }
        }

        #region Helpers

        private GameObject SpawnTile(int i, int j, Transform where, BiomeType biome, TileType type)
        {
            Vector3 pos = Utils.GetVector3FromMatrixPos(i, j, _cellSize) - _offset;
            GameObject tile = _generationStrategy(type, biome, i, j);

            if (tile.IsNotNull())
            {
                GameObject spawnedTile = GameObject.Instantiate(tile, where);
                spawnedTile.transform.localPosition = pos;
                return spawnedTile;
            }

            return null;
        }

        private void SpawnTile(int i, int j, Transform where, BiomeType biome, TileType tyle, TileType[,] layer)
        {
            GameObject spawnedObject = SpawnTile(i, j, where, biome, tyle);

            if (spawnedObject.IsNull())
            {
                return;
            }

            TileObject tile = spawnedObject.GetComponent<TileObject>();

            if (tile.IsNull())
            {
                return;
            }

            tile.LayerPosition = new LayerPosition(new Vector2Int(i, j), layer);

            if (tile is IDataSavingTile data)
            {
                data.ObjectName = tile.name.Replace("(Clone)", ""); // remove the clone from name after instantiate
            }

            _afterTileSpawnedAction?.Invoke(tile, biome, i, j);
        }

        private void SpawnTileIf(int i, int j, Transform where, BiomeType biome, TileType type, TileType condition)
        {
            if (type == condition)
            {
                SpawnTile(i, j, where, biome, type);
            }
        }

        private TileSettings GetSettings(BiomeType biome) => biome switch
        {
            BiomeType.Fire => _fireTiles,
            BiomeType.Dungeon => _dungeonTiles,
            BiomeType.Grassland => _grassTiles,
            _ => null,
        };

        private void SetOffset(TileType[,] layer)
        {
            int middlePos = layer.GetLength(0) / 2;
            _offset = Utils.GetVector3FromMatrixPos(middlePos, middlePos, _cellSize);
        }

        private GameObject RandomGeneration(TileType tileType, BiomeType biomeType, int x, int y)
        {
            return GetSettings(biomeType).GetTile(tileType);
        }

        private GameObject GenerateFromSave(TileType tileType, BiomeType biomeType, int x, int y)
        {
            LayerSaveData layer = SaveData.GetFromBiome(biomeType);
            Vector2IntPos key = new Vector2IntPos(x, y);

            string name = layer.DynamicObjects[key].ObjectName;

            return GetSettings(biomeType).GetTileFromName(tileType, name);
        }

        private void AfterTileLoaded(TileObject tile, BiomeType biomeType, int x, int y)
        {
            if (tile is IDataSavingTile loadingObject)
            {
                LayerSaveData layer = SaveData.GetFromBiome(biomeType);
                Vector2IntPos key = new Vector2IntPos(x, y);

                if (!layer.DynamicObjects.ContainsKey(key))
                {
                    return;
                }

                ObjectSaveData saveData = layer.DynamicObjects[key];
                loadingObject.LoadFromSave(saveData);
            }
        }

        #endregion
    }
}
