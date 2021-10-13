using Core;
using UnityEngine;

namespace Gameplay.Generation
{
    public class LayersSpawner : MonoBehaviour
    {
        [SerializeField] private int _layerSize;
        [SerializeField] private float _cellSize;
        [SerializeField] private TileSettings _tilesSettings;

        private LayersGenerator _generator;

        private void Start()
        {
            _generator = new LayersGenerator(_layerSize);

            GenerateLayers(new Room(Vector2Int.zero, null, RoomType.Normal, 0), Vector3.zero);
        }

        public void GenerateLayers(Room room, Vector3 where)
        {
            SpawnBackground(room, where);

            switch (room.Type)
            {
                case RoomType.Empty:
                    break;
                case RoomType.Normal:
                    SpawnNormal(room, where);
                    break;
                case RoomType.Start:
                    break;
                case RoomType.End:
                    SpawnEnd(room, where);
                    break;
                case RoomType.Healing:
                    SpawnHealing(room, where);
                    break;
                case RoomType.Chest:
                    SpawnChest(room, where);
                    break;
                case RoomType.Wall:
                    break;
                default:
                    break;
            }
        }

        private void SpawnNormal(Room room, Vector3 where)
        {
            Layers layers = _generator.Generate();

            TileType[,] mainLayer = layers.GetTiles(1);
            int size = mainLayer.GetLength(0);
            Vector3 offset = new Vector3(size / 2 * _cellSize, size / 2 * _cellSize);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Vector3 pos = new Vector3(_cellSize * i, _cellSize * j) - offset;
                    GameObject tile = _tilesSettings.GetTile(mainLayer[i, j]);

                    if (!ReferenceEquals(tile, null))
                    {
                        Instantiate(tile, where + pos, Quaternion.identity);
                    }
                }
            }
        }

        private void SpawnEnd(Room room, Vector3 where)
        {

        }

        private void SpawnHealing(Room room, Vector3 where)
        {

        }

        private void SpawnChest(Room room, Vector3 where)
        {

        }

        private void SpawnBackground(Room room, Vector3 where)
        {

        }
    }
}
