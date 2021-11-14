using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;
using Utilities;
using Gameplay.Events;
using Values;

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
        [SerializeField] private FloatValue _cellSize;
        [Tooltip("lenght should be odd")]
        [SerializeField] private int _length;
        [SerializeField] private int _maxRoomCount;
        [SerializeField] private int _cellCount;
        [Header("Events")]
        [SerializeField] private RoomBehaviourEvent _roomBehaviourEvent;

        private RoomTraverser<RoomBehaviour> _traverser;
        private DungeonGenerator _generator;

        public void Spawn()
        {
            _generator = new DungeonGenerator(_maxRoomNeighbours, _maxRoomCount, _length);
            Room start = _generator.GenerateDungeon();
            _traverser = new RoomTraverser<RoomBehaviour>(start, _generator.Matrix.GetLength(0));

            GenerateRoomTypes();
            GenerateGameAssests();
            GenereteLayers();
            SetDoorPositions();
            SpawnLayers();
            SpawnDoors();

            _roomBehaviourEvent.Value = _traverser.Start;
        }

        private void GenerateRoomTypes()
        {
            _generator.CalculateDistances();
            var distances = _generator.Distances;
            distances[distances.Count - 1].room.Type = RoomType.End;
            distances[0].room.Type = RoomType.Start;

            ChooseRoomRandom(RoomType.Healing);
            ChooseRoomRandom(RoomType.Chest);

            void ChooseRoomRandom(RoomType type)
            {
                Room room;

                do
                {
                    room = distances[Random.Range(1, distances.Count - 1)].room;
                } while (room.Type != RoomType.Normal);

                room.Type = type;
            }
        }

        private GameObject SpawnRoom(Room room, Vector3 where)
        {
            Vector2Int pos = room.Pos;

            if (_traverser[pos].IsNull())
            {
                RoomBehaviour result = Instantiate(_room, where, Quaternion.identity);

                _traverser[room.Pos] = result;

                return result.gameObject;
            }

            return null;
        }

        private void GenerateGameAssests()
        {
            _traverser.Traverse(room =>
            {
                RoomBehaviour behavior = _traverser[room.Pos];

                Vector3 where = Vector3.zero;

                if (room.LastRoom != null)
                {
                    Vector3 direction = Utils.GetWorldDirection(room.Direction) * _distance;
                    where = _traverser[room.LastRoom.Pos].transform.position + direction;
                }

                SpawnRoom(room, where);
            });
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
            LayersSpawner spawner = new LayersSpawner(_cellSize.Value, _tileSettings);

            _traverser.TraverseUnique(room =>
            {
                RoomBehaviour behaviour = _traverser[room.Pos];
                spawner.GenerateLayers(behaviour);
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
            Vector2Int where = GetLayerPosition(direction, layer);
            layer[where.x, where.y] = TileType.None;
        }

        private Vector2Int GetLayerPosition(Vector2Int direction, TileType[,] layer)
        {
            int size = layer.GetLength(0);
            int middle = size / 2;
            Vector2Int middlePos = new Vector2Int(middle, middle);
            return middlePos + (direction * middle);
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

                DoorBehaviour currentDoor = Instantiate(_door, currentRoom.GetTransform(currentRoom.Layers.MiddleLayerIndex));
                DoorBehaviour previousDoor = Instantiate(_door, previousRoom.GetTransform(previousRoom.Layers.MiddleLayerIndex));

                SetDoor(currentDoor, previousDoor, room.Pos - room.LastRoom.Pos, currentRoom);
                SetDoor(previousDoor, currentDoor, room.LastRoom.Pos - room.Pos, previousRoom);
            });
        }

        private void SetDoor(DoorBehaviour door, DoorBehaviour other, Vector2Int direction, RoomBehaviour room)
        {
            Vector3 spawnDirection = new Vector3(-direction.y, direction.x) * _cellSize.Value;
            Vector3 movePoint = spawnDirection * (_cellCount / 2 - 1);

            movePoint += room.transform.position;
            door.transform.localPosition = spawnDirection * (_cellCount / 2);

            TileType[,] layer = room.Layers[room.CurrentLayer];
            LayerPosition layerPosition = new LayerPosition(GetLayerPosition(direction, layer) - direction, layer);

            door.Set(movePoint, other, room, layerPosition);
        }
    }
}