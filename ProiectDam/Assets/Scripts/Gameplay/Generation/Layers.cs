using Gameplay.DataSaving;
using System.Collections;
using System.Collections.Generic;

namespace Gameplay.Generation
{
    public class Layers : IEnumerable<Layers.LayerData>
    {
        public struct LayerData
        {
            public LayerData(TileType[,] tiles, BiomeType biome)
            {
                Tiles = tiles;
                Biome = biome;
            }

            public TileType[,] Tiles { get; }
            public BiomeType Biome { get; }
        }

        private readonly List<LayerData> _layers;
        private readonly int _layerSize;

        public Layers(int layerSize, int numberOfLayers)
        {
            _layers = new List<LayerData>(numberOfLayers);
            _layerSize = layerSize;

            for (int i = 0; i < numberOfLayers; i++)
            {
                _layers.Add(new LayerData(new TileType[layerSize, layerSize], GetBiome(i, numberOfLayers)));
            }
        }

        public void SetFromSave(LayersSaveData saveData)
        {
            _layers.Clear();

            foreach (var layer in saveData.Layers)
            {
                _layers.Add(new LayerData(layer.Layers, layer.Biome));
            }
        }

        #region Properties

        public int LayerSize => _layerSize;
        public int Count => _layers.Count;
        public int MiddleIndex => _layerSize / 2;
        public int MiddleLayerIndex => _layers.Count / 2;

        public TileType[,] this[int layer] => _layers[layer].Tiles;

        public TileType[,] Middle => _layers[_layers.Count / 2].Tiles;

        public TileType[,] GetTiles(int layer) => _layers[layer].Tiles;

        public BiomeType GetBiome(int index) => _layers[index].Biome;

        #endregion

        #region Methods

        private BiomeType GetBiome(int index, int max)
        {
            if (max <= 1)
            {
                return BiomeType.Grassland;
            }

            return (BiomeType)(index + 1);
        }

        #endregion

        #region Enumerators

        public List<LayerData>.Enumerator GetEnumerator()
            => _layers.GetEnumerator();

        IEnumerator<LayerData> IEnumerable<LayerData>.GetEnumerator()
            => _layers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _layers.GetEnumerator();

        #endregion
    }
}
