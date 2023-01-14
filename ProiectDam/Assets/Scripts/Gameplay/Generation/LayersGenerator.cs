using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Gameplay.Generation
{
    public class LayersGenerator
    {
        #region Constants

        private static readonly TileType[] ObjectsTypes = 
        { 
            TileType.None, TileType.Enemy, TileType.PickUp, TileType.Obstacle, TileType.Trap 
        };

        private static readonly HashSet<TileType> ObstaclesTypes = new()
        { 
            TileType.Enemy, TileType.Obstacle, TileType.Trap, TileType.DynamicObstacle 
        };

        #endregion

        private readonly int _layerSize;
        private readonly Vector2Int _enemiesRange;

        public LayersGenerator(int layerSize, Vector2Int enemiesRange)
        {
            _layerSize = layerSize;
            _enemiesRange = enemiesRange;
        }

        public Layers Generate(Room room) => room.Type switch
        {
            RoomType.Normal => GenerateNormal(),
            RoomType.Start => GenerateEmpty(),
            RoomType.End => GenerateInMiddle(TileType.Portal),
            RoomType.Empty => GenerateEmpty(),
            RoomType.Healing => GenerateHeal(),
            RoomType.Chest => GenerateChest(),
            RoomType.Merchant => GenerateMerchant(),
            RoomType.Boss => GenerateBoss(),
            _ => GenerateEmpty(),
        };

        private Layers GenerateEmpty()
        {
            Layers layers = new(_layerSize, 1);

            GenerateBorder(layers.GetTiles(0));

            return layers;
        }

        private Layers GenerateMerchant()
        {
            Layers layers = GenerateEmpty();
            TileType[,] layer = layers.GetTiles(0);

            SetType(layer, TileType.Merchant);
            SetType(layer, TileType.DynamicObstacle);
            SetType(layer, TileType.DynamicObstacle);

            return layers;
        }

        private Layers GenerateBoss()
        {
            Layers layers = new(_layerSize, 3);

            foreach (var layer in layers)
            {
                List<Vector2Int> positions = GetPositions(layer.Tiles);

                GenerateBorder(layer.Tiles);
                SpawnTiles(layer.Tiles, positions, 1, TileType.Enemy);
                SpawnTiles(layer.Tiles, GetPositions(layer.Tiles), Random.Range(3, 6), TileType.Obstacle);
            }

            return layers;
        }

        private Layers GenerateChest()
        {
            Layers layers = GenerateEmpty();

            TileType[,] tiles = layers.GetTiles(0);

            SetType(tiles, TileType.Chest);
            SetType(tiles, TileType.Heal);
            SetType(tiles, TileType.PickUp);

            return layers;
        }

        private void SetType(TileType[,] tiles, TileType type)
        {
            int size = tiles.GetLength(0);
            int x, y;
            do
            {
                x = Random.Range(2, size - 2);
                y = Random.Range(2, size - 2);
            } while (tiles[x, y] != TileType.None);

            tiles[x, y] = type;
        }

        private Layers GenerateInMiddle(TileType type)
        {
            Layers layers = GenerateEmpty();
            int middle = layers.MiddleIndex;
            layers.GetTiles(0)[middle, middle] = type;

            return layers;
        }

        private Layers GenerateHeal()
        {
            Layers layers = GenerateEmpty();
            TileType[,] tiles = layers.GetTiles(0);

            int healCount = Random.Range(2, 5);

            for (int i = 0; i < healCount; i++)
            {
                SetType(tiles, TileType.Heal);
            }

            return layers;
        }

        private Layers GenerateNormal()
        {
            Layers layers = new Layers(_layerSize, 3);

            GenerateSimple(layers.GetTiles(1));

            GenerateComplex(layers.GetTiles(0), layers.GetTiles(1));
            GenerateComplex(layers.GetTiles(2), layers.GetTiles(1));

            return layers;
        }

        private int GetEnemiesCountRandom()
        {
            return Random.Range(_enemiesRange.x, _enemiesRange.y + 1);
        }

        private void GenerateSimple(TileType[,] tiles)
        {
            GenerateBorder(tiles);

            List<Vector2Int> positions = GetPositions(tiles);

            SpawnTiles(tiles, positions, GetEnemiesCountRandom(), TileType.Enemy);
            SpawnTiles(tiles, positions, Random.Range(2, 3), TileType.PickUp);
            SpawnTiles(tiles, positions, Random.Range(3, 7), TileType.Obstacle);
            SpawnTiles(tiles, positions, 1, TileType.Trap);
            SpawnTiles(tiles, positions, Random.Range(0, 3), TileType.DynamicObstacle);
        }

        private void GenerateComplex(TileType[,] current, TileType[,] previous)
        {
            GenerateBorder(current);

            List<Vector2Int> positions = GetPositions(current);
            
            SpawnTilesComplex(current, previous, positions, GetEnemiesCountRandom(), TileType.Enemy);
            SpawnTilesComplex(current, previous, positions, Random.Range(2, 3), TileType.PickUp);
            SpawnTilesComplex(current, previous, positions, Random.Range(3, 7), TileType.Obstacle);
            SpawnTilesComplex(current, previous, positions, 1, TileType.Trap);
            SpawnTilesComplex(current, previous, positions, Random.Range(0, 3), TileType.DynamicObstacle);
        }

        private void GenerateBorder(TileType[,] tiles)
        {
            int size = tiles.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                tiles[0, i] = TileType.Wall;
                tiles[size - 1, i] = TileType.Wall;
                tiles[i, 0] = TileType.Wall;
                tiles[i, size - 1] = TileType.Wall;
            }

            for (int i = 1; i < size - 1; i++)
            {
                tiles[1, i] = TileType.None;
                tiles[size - 2, i] = TileType.None;
                tiles[i, 1] = TileType.None;
                tiles[i, size - 2] = TileType.None;
            }
        }

        private List<Vector2Int> GetPositions(TileType[,] layer)
        {
            List<Vector2Int> list = new();
            int size = layer.GetLength(0) - 2;

            for (int i = 2; i < size; i++)
            {
                for (int j = 2; j < size; j++)
                {
                    if (layer[i, j] is not TileType.None)
                    {
                        continue;
                    }

                    list.Add(new Vector2Int(i, j));
                }
            }

            return list;
        }

        private void SpawnTiles(TileType[,] layer, List<Vector2Int> positions, int count, TileType type)
        {
            for (int i = 0; i < count; i++)
            {
                int where = Random.Range(0, positions.Count);
                Vector2Int position = positions[where];

                positions.RemoveAt(where);
                layer[position.x, position.y] = type;
            }
        }

        private void SpawnTilesComplex(TileType[,] layer, TileType[,] previous, List<Vector2Int> positions, int count, TileType type)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2Int position;

                do
                {
                    if (positions.Count < 1)
                    {
                        return;
                    }

                    int where = Random.Range(0, positions.Count);
                    position = positions[where];

                    positions.RemoveAt(where);
                } while (ObstaclesTypes.Contains(previous[position.x, position.y]));

                layer[position.x, position.y] = type;
            }
        }
    }
}
