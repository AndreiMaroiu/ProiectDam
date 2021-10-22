using Core;
using Gameplay.Events;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Gameplay.Generation
{
    internal sealed class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private int _maxRoomNeighbours;
        [SerializeField] private float _distance;
        [Header("Prefabs")]
        [SerializeField] private RoomBehaviour _room;
        [SerializeField] private DoorBehaviour _door;
        [SerializeField] private TileSettings _tileSettings;
        [Header("Settings")]
        [SerializeField] private float _cellSize;
        [Tooltip("lenght should be odd")]
        [SerializeField] private int _length;
        [SerializeField] private int _maxRoomCount;
        [SerializeField] private int _cellCount;
        [Header("Events")]
        [SerializeField] private RoomBehaviourEvent _roomBehaviourEvent;

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
            SpawnDoors();

            _roomBehaviourEvent.Value = _traverser.Start;
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

            if (_traverser[pos].IsNull())
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

                if (!spawnedRoom.IsNull())
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
            LayersGenerator generator = new LayersGenerator(_cellCount);
            
            _traverser.Traverse(room =>
            {
                RoomBehaviour behaviour = _traverser[room.Pos];

                if (behaviour.Layers is null)
                {
                    behaviour.Set(room, generator.Generate(room));
                }
            });
        }

        private void SpawnLayers()
        {
            LayersSpawner spawner = new LayersSpawner(_cellSize, _tileSettings);

            _traverser.TraverseUnique(room =>
            {
                RoomBehaviour behaviour = _traverser[room.Pos];

                spawner.GenerateLayers(behaviour);
                behaviour.AreLayersSpawned = true;
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

                SetDoorPosition(room.Pos - room.LastRoom.Pos, current.Layers.Middle);
                SetDoorPosition(room.LastRoom.Pos - room.Pos, previous.Layers.Middle);
            });
        }

        private void SetDoorPosition(Vector2Int direction, TileType[,] layer)
        {
            int size = layer.GetLength(0);
            int middle = size / 2;
            Vector2Int middlePos = new Vector2Int(middle, middle);
            Vector2Int where = middlePos + (direction * middle);

            layer[where.x, where.y] = TileType.None;
        }

        private void SpawnDoors()
        {
            _traverser.Traverse(room =>
            {
                if (room.LastRoom is null)
                {
                    return;
                }

                RoomBehaviour currentRoom = _traverser[room.Pos];
                RoomBehaviour previousRoom = _traverser[room.LastRoom.Pos];

                DoorBehaviour currentDoor = Instantiate(_door, currentRoom.transform);
                DoorBehaviour previousDoor = Instantiate(_door, previousRoom.transform);

                SetDoor(currentDoor, previousDoor, room.Pos - room.LastRoom.Pos, currentRoom);
                SetDoor(previousDoor, currentDoor, room.LastRoom.Pos - room.Pos, previousRoom);
            });
        }

        private void SetDoor(DoorBehaviour door, DoorBehaviour other, Vector2 direction, RoomBehaviour room)
        {
            Vector3 movePoint = new Vector3(-direction.y, direction.x) * (_cellCount / 2 - 1);
            movePoint += room.transform.position;

            door.transform.localPosition = new Vector3(-direction.y, direction.x) * (_cellCount / 2);

            door.Set(movePoint, other, room);
        }
    }
}