using System.Collections;
using System.Collections.Generic;

namespace Gameplay.Generation
{
    public class Layers : IEnumerable<TileType[,]>
    {
        private readonly List<TileType[,]> _layers;
        private readonly List<BiomeType> _biomes;
        private readonly int _layerSize;

        public Layers(int layerSize, int numberOfLayers)
        {
            _layers = new List<TileType[,]>(numberOfLayers);
            _biomes = new List<BiomeType>(numberOfLayers);
            _layerSize = layerSize;

            for (int i = 0; i < numberOfLayers; i++)
            {
                _layers.Add(new TileType[layerSize, layerSize]);
                _biomes.Add(GetBiome(i, numberOfLayers));
            }
        }

        #region Properties

        public int LayerSize => _layerSize;
        public int Count => _layers.Count;
        public int MiddleIndex => _layerSize / 2;
        public int MiddleLayerIndex => _layers.Count / 2;

        public TileType[,] this[int layer] => _layers[layer];

        public TileType[,] Middle => _layers[_layers.Count / 2];

        public TileType[,] GetTiles(int layer) => _layers[layer];

        public BiomeType GetBiome(int index) => _biomes[index];

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

        public List<TileType[,]>.Enumerator GetEnumerator()
            => _layers.GetEnumerator();

        IEnumerator<TileType[,]> IEnumerable<TileType[,]>.GetEnumerator()
            => _layers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _layers.GetEnumerator();

        #endregion
    }
}
