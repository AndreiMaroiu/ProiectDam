using System.Collections;
using System.Collections.Generic;

namespace Gameplay.Generation
{
    public class Layers : IEnumerable<TileType[,]>
    {
        private readonly List<TileType[,]> _layers;
        private readonly int _layerSize;

        public Layers(int layerSize, int numberOfLayers)
        {
            _layers = new List<TileType[,]>();
            _layerSize = layerSize;

            for (int i = 0; i < numberOfLayers; i++)
            {
                _layers.Add(new TileType[layerSize, layerSize]);
            }
        }

        public int LayerSize => _layerSize;
        public int Count => _layers.Count;
        public int MiddleIndex => _layerSize / 2;
        public int MiddleLayerIndex => _layers.Count / 2;

        public TileType[,] this[int index] => _layers[index];

        public TileType[,] Middle => _layers[_layers.Count / 2];

        public TileType[,] GetTiles(int layer)
            => _layers[layer];

        public List<TileType[,]>.Enumerator GetEnumerator()
            => _layers.GetEnumerator();

        IEnumerator<TileType[,]> IEnumerable<TileType[,]>.GetEnumerator()
            => _layers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _layers.GetEnumerator();
    }
}
