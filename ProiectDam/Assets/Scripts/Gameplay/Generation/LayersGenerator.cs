using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Gameplay.Generation
{
    public class LayersGenerator
    {
        #region Constants

        private static readonly TileType[] ObjectsTypes = new TileType[]
        { TileType.None, TileType.Grass, TileType.Enemy, TileType.PickUp, TileType.Obstacle, TileType.Heal, TileType.Trap };

        private static readonly int[] Changes = new int[]
        { 40, 20, 5, 5, 20, 3, 7 };

        private static readonly HashSet<TileType> ObstaclesTypes = new HashSet<TileType>()
        { TileType.Enemy, TileType.Obstacle, TileType.Trap };

        #endregion

        private readonly int _layerSize;
        private readonly WeightedRandom<TileType> _random;

        public LayersGenerator(int layerSize)
        {
            _layerSize = layerSize;
            _random = new WeightedRandom<TileType>(ObjectsTypes, Changes);
        }

        public Layers Generate(Room room)
        {
            Layers layers = new Layers(_layerSize, 3);

            GenerateSimple(layers.GetTiles(1));

            GenerateComplex(layers.GetTiles(0), layers.GetTiles(1));
            GenerateComplex(layers.GetTiles(2), layers.GetTiles(1));

            return layers;
        }

        private void GenerateSimple(TileType[,] tiles)
        {
            GenerateBorder(tiles);

            int size = tiles.GetLength(0);

            // counting starts from 2 to ignore wall and empty path
            for (int i = 2; i < size - 2; i++)
            {
                for (int j = 2; j < size - 2; j++)
                {
                    tiles[i, j] = _random.Take();
                }
            }
        }

        private void GenerateComplex(TileType[,] current, TileType[,] previous)
        {
            GenerateBorder(current);

            int size = current.GetLength(0);

            // counting starts from 2 to ignore wall and empty path
            for (int i = 2; i < size - 2; i++)
            {
                for (int j = 2; j < size - 2; j++)
                {
                    if (ObstaclesTypes.Contains(previous[i, j]))
                    {
                        current[i, j] = TileType.None;
                    }
                    else
                    {
                        current[i, j] = ObjectsTypes[Random.Range(0, ObjectsTypes.Length)];
                    }
                }
            }
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
    }
}
