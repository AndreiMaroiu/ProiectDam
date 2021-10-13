namespace Gameplay.Generation
{
    public class Layers
    {
        private readonly TileType[,] _backLayer;
        private readonly TileType[,] _currentLayer;
        private readonly TileType[,] _frontLayer;

        public Layers(int layerSize)
        {
            int size = layerSize + 4;
            _backLayer = new TileType[size, size];
            _currentLayer = new TileType[size, size];
            _frontLayer = new TileType[size, size];
        }

        public TileType[,] GetTiles(int layer) => layer switch
        {
            0 => _backLayer,
            1 => _currentLayer,
            2 => _frontLayer,
            _ => default,
        };
    }
}
