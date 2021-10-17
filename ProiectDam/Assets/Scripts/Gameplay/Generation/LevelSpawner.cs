using Core;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Generation
{
    internal sealed class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private int _maxRoomNeighbours;
        [SerializeField] private float _distance;
        [Space]
        [SerializeField] private RoomBehaviour _room;
        [SerializeField] private GameObject _door;
        [SerializeField] private TileSettings _tileSettings;
        [SerializeField] private float _cellSize;
        [Tooltip("lenght should be odd")]
        [SerializeField] private int _length;
        [SerializeField] private int _maxRoomCount;

        private RoomTraverser<RoomBehaviour> _traverser;

        private void Start()
        {
            DungeonGenerator generator = new DungeonGenerator(_maxRoomNeighbours, _maxRoomCount, _length);
            Room start = generator.GenerateDungeon();

            _traverser = new RoomTraverser<RoomBehaviour>(start, generator.Matrix.GetLength(0));

            GenerateGameAssests(start);
            GenereteLayers();
            SetDoorPositions();
            SpawnLayers();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }

        private GameObject SpawnRoom(Room room, Vector3 where)
        {
            Vector2Int pos = room.Pos;

            if (ReferenceEquals(_traverser[pos], null))
            {
                RoomBehaviour result = Instantiate(_room, where, Quaternion.identity);

                _traverser[room.Pos] = result;

                Color color = room.Type switch
                {
                    RoomType.End => Color.blue,
                    RoomType.Start => Color.green,
                    _ => Color.white,
                };

                result.GetComponent<SpriteRenderer>().color = color;

                return result.gameObject;
            }

            return null;
        }

        private void GenerateGameAssests(Room start)
        {
            Queue<(Room room, Vector3 pos)> queue = new Queue<(Room, Vector3)>();
            GameObject lastRoom = null;

            queue.Enqueue((start, Vector3.zero));

            while (queue.Count > 0)
            {
                var top = queue.Dequeue();

                GameObject spawnedRoom = SpawnRoom(top.room, top.pos);

                if (!ReferenceEquals(spawnedRoom, null))
                {
                    lastRoom = spawnedRoom;
                }

                foreach (Room room in top.room)
                {
                    queue.Enqueue((room, top.pos + (Utils.GetWorldDirection(room.Direction) * _distance)));
                }
            }

            lastRoom.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        private void GenereteLayers()
        {
            LayersGenerator generator = new LayersGenerator(5);
            
            _traverser.Traverse(room =>
            {
                RoomBehaviour behaviour = _traverser[room.Pos];

                if (behaviour.Layers == null)
                {
                    behaviour.Set(room, generator.Generate(room));
                }
            });
        }

        private void SpawnLayers()
        {
            LayersSpawner spawner = new LayersSpawner(_cellSize, _tileSettings);

            _traverser.Traverse(pos =>
            {
                RoomBehaviour behaviour = _traverser[pos];

                if (!behaviour.AreLayersSpawned)
                {
                    spawner.GenerateLayers(behaviour);
                    behaviour.AreLayersSpawned = true;
                }
            });
        }

        private void SetDoorPositions()
        {
            _traverser.Traverse(room =>
            {
                if (room.LastRoom is null)
                {
                    return;
                }

                RoomBehaviour current = _traverser[room.Pos];
                RoomBehaviour previous = _traverser[room.LastRoom.Pos];

                SetDoor(room.Pos - room.LastRoom.Pos, current.Layers.Middle);
                SetDoor(room.LastRoom.Pos - room.Pos, previous.Layers.Middle);
            });
        }

        private void SetDoor(Vector2Int direction, TileType[,] layer)
        {
            int size = layer.GetLength(0);
            int middle = size / 2;
            Vector2Int middlePos = new Vector2Int(middle, middle);
            Vector2Int where = middlePos + (direction * middle);

            layer[where.x, where.y] = TileType.None;
        }
    }
}