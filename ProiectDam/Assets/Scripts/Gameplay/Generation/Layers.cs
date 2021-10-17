using System.Collections;
using System.Collections.Generic;

namespace Gameplay.Generation
{
    public class Layers: IEnumerable<TileType[,]>
    {
        private readonly List<TileType[,]> _layers;

        public Layers(int layerSize, int numberOfLayers)
        {
            int size = layerSize + 4;
            _layers = new List<TileType[,]>();

            for (int i = 0; i < numberOfLayers; i++)
            {
                _layers.Add(new TileType[size, size]);
            }
        }

        public int Count => _layers.Count;

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
