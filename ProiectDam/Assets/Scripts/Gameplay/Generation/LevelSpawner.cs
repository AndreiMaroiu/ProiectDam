using Core;
using Gameplay.Events;
using UnityEngine;
using Utilities;
using Core.Values;

namespace Gameplay.Generation
{
    /// <summary>
    /// Main Behaviour to generate the whole level data
    /// </summary>
    internal sealed class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private int _maxRoomNeighbours;
        [SerializeField] private float _distance;
        [Header("Prefabs")]
        [SerializeField] private RoomBehaviour _room;
        [SerializeField] private DoorBehaviour _door;
        [Header("Tiles")]
        [SerializeField] private TileSettings _dungeonTiles;
        [SerializeField] private TileSettings _fireTiles;
        [SerializeField] private TileSettings _grassTiles;
        [Header("Settings")]
        [SerializeField] private FloatValue _cellSize;
        [Tooltip("lenght should be odd")]
        [SerializeField] private IntValue _maxtrixSize;
        [SerializeField] private int _maxRoomCount;
        [SerializeField] private int _cellCount;
        [Header("Events")]
        [SerializeField] private RoomBehaviourEvent _roomBehaviourEvent;

        private RoomTraverser<RoomBehaviour> _traverser;
        private DungeonGenerator _generator;

        /// <summary>
        /// Main spawn point function
        /// </summary>
        public void Spawn()
        {
            GenerateDungeon();
            GenerateRoomTypes();
            SpawnRoomAssets();
            GenereteLayers();
            SetDoorPositions();
            SpawnLayers();
            SpawnDoors();

            _roomBehaviourEvent.Value = _traverser.Start;
        }

        /// <summary>
        /// Generate dungeon room layout (only room positions)
        /// </summary>
        private void GenerateDungeon()
        {
            _generator = new DungeonGenerator(_maxRoomNeighbours, _maxRoomCount, _maxtrixSize);
            Room start = _generator.GenerateDungeon();
            _traverser = new RoomTraverser<RoomBehaviour>(start, _generator.Matrix.GetLength(0));
        }

        /// <summary>
        /// Choose random some room types such as heal, chest etc.
        /// </summary>
        private void GenerateRoomTypes()
        {
            _generator.CalculateDistances();
            var duplicates = _generator.CalculateDuplicates();
            var distances = _generator.Distances;
            Validate(distances[distances.Count - 1].room.Pos, RoomType.End);

            distances[0].room.Type = RoomType.Start;

            ChooseRoomRandom(RoomType.Healing);
            ChooseRoomRandom(RoomType.Chest);
            ChooseRoomRandom(RoomType.Merchant);

            void Validate(Vector2Int pos, RoomType type)
            {
                foreach (Room room in duplicates[pos])
                {
                    room.Type = type;
                }
            }

            void ChooseRoomRandom(RoomType type)
            {
                Room room;
                int halfCount = distances.Count / 2;
                int countMinus = distances.Count - 1;

                do
                {
                    int distance = Random.Range(halfCount, countMinus);
                    room = distances[distance].room;
                } while (room.Type != RoomType.Normal);

                Validate(room.Pos, type);
            }
        }

        /// <summary>
        /// Spawn gameobjects for a single room
        /// </summary>
        /// <param name="room">room data</param>
        /// <param name="where">world position where to spawn the room</param>
        /// <returns>the gameobject of the room</returns>
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

        /// <summary>
        /// For each room in dungeon spawn, spawn their game objects
        /// </summary>
        private void SpawnRoomAssets()
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


        /// <summary>
        /// For each room in dungeon, based on it's RoomType, generate room layers (tiles for each dimension)
        /// </summary>
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

        /// <summary>
        /// For each room, spawn the game objects based on room layout (tiles)
        /// </summary>
        private void SpawnLayers()
        {
            LayersSpawner spawner = new LayersSpawner(_cellSize.Value, _grassTiles, _fireTiles, _dungeonTiles);

            _traverser.TraverseUnique(room =>
            {
                RoomBehaviour behaviour = _traverser[room.Pos];
                spawner.GenerateLayers(behaviour);
            });
        }

        /// <summary>
        /// For each room set door position. Door position are independent of the room layout.
        /// </summary>
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

                SetDoorPosition(room.Pos - room.LastRoom.Pos, current.Layers.Middle, TileType.Door);
                SetDoorPosition(room.LastRoom.Pos - room.Pos, previous.Layers.Middle, TileType.Door);
            });
        }


        private void SetDoorPosition(Vector2Int direction, TileType[,] layer, TileType type)
        {
            Vector2Int where = GetLayerPosition(direction, layer);
            layer[where.x, where.y] = type;
        }

        /// <summary>
        /// Get the furthest position of a layer based on a direction
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        private Vector2Int GetLayerPosition(Vector2Int direction, TileType[,] layer)
        {
            int size = layer.GetLength(0);
            int middle = size / 2;
            Vector2Int middlePos = new Vector2Int(middle, middle);
            return middlePos + (direction * middle);
        }

        /// <summary>
        /// Spawn door assets for each room. Doors also have a rotation
        /// </summary>
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

        /// <summary>
        /// Sets necessary data to connect two doors
        /// </summary>
        /// <param name="door">first door</param>
        /// <param name="other">second door</param>
        /// <param name="direction">direction between doors</param>
        /// <param name="room">room of the first door</param>
        private void SetDoor(DoorBehaviour door, DoorBehaviour other, Vector2Int direction, RoomBehaviour room)
        {
            Vector3 spawnDirection = Utils.GetVector3FromMatrixPos(direction, _cellSize.Value);
            Vector3 movePoint = spawnDirection * (_cellCount / 2 - 1);

            movePoint += room.transform.position;
            door.transform.localPosition = spawnDirection * (_cellCount / 2);

            TileType[,] layer = room.Layers[room.CurrentLayer];
            LayerPosition layerPosition = new LayerPosition(GetLayerPosition(direction, layer) - direction, layer);

            door.Set(movePoint, other, room, layerPosition);

            door.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        }
    }
}