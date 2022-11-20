using Core;
using Core.Values;
using Gameplay.Events;
using UnityEngine;
using Utilities;

namespace Gameplay.Generation
{
    public abstract class BaseLevelSpawner : MonoBehaviour
    {
        [SerializeField] protected LevelGeneratorData _data;
        [SerializeField] protected Vector2Int _enemiesRange;

        protected RoomTraverser<RoomBehaviour> _traverser;

        public RoomTraverser<RoomBehaviour> Traverser => _traverser;

        /// <summary>
        /// Main spawn point function
        /// </summary>
        public abstract void Spawn();

        /// <summary>
        /// Spawn gameobjects for a single room
        /// </summary>
        /// <param name="room">room data</param>
        /// <param name="where">world position where to spawn the room</param>
        /// <returns>the gameobject of the room</returns>
        protected GameObject SpawnRoom(Room room, Vector3 where)
        {
            Vector2Int pos = room.Pos;

            if (_traverser[pos].IsNull())
            {
                RoomBehaviour result = Instantiate(_data.Room, where, Quaternion.identity);

                _traverser[room.Pos] = result;

                return result.gameObject;
            }

            return null;
        }

        /// <summary>
        /// For each room in dungeon spawn, spawn their game objects
        /// </summary>
        protected void SpawnRoomAssets()
        {
            _traverser.Traverse(room =>
            {
                Vector3 where = Vector3.zero;

                if (room.LastRoom != null)
                {
                    Vector3 direction = Utils.GetWorldDirection(room.Direction) * _data.Distance;
                    where = _traverser[room.LastRoom.Pos].transform.position + direction;
                }

                room.GameObject = SpawnRoom(room, where);
            });
        }


        /// <summary>
        /// For each room in dungeon, based on it's RoomType, generate room layers (tiles for each dimension)
        /// </summary>
        protected void GenereteLayers()
        {
            LayersGenerator generator = new LayersGenerator(_data.CellCount, _enemiesRange);

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
        protected void SpawnLayers()
        {
            LayersSpawner spawner = new LayersSpawner(_data.CellSize, _data.GrassTiles, _data.FireTiles, _data.DungeonTiles);

            _traverser.TraverseUnique(room =>
            {
                RoomBehaviour behaviour = _traverser[room.Pos];
                spawner.SpawnStatic(behaviour);
                spawner.SpawnDynamic(behaviour);
            });
        }

        /// <summary>
        /// For each room set door position. Door position are independent of the room layout.
        /// </summary>
        protected void SetDoorPositions()
        {
            _traverser.Traverse(room =>
            {
                if (room.LastRoom is null)
                {
                    return;
                }

                RoomBehaviour current = _traverser[room.Pos];
                RoomBehaviour previous = _traverser[room.LastRoom.Pos];

                SetDoorPosition(room.LastRoom.Pos - room.Pos, current.Layers.Middle, TileType.Door);
                SetDoorPosition(room.Pos - room.LastRoom.Pos, previous.Layers.Middle, TileType.Door);
            });
        }


        protected void SetDoorPosition(Vector2Int direction, TileType[,] layer, TileType type)
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
        protected Vector2Int GetLayerPosition(Vector2Int direction, TileType[,] layer)
        {
            int size = layer.GetLength(0);
            int middle = size / 2;
            Vector2Int middlePos = new Vector2Int(middle, middle);
            return middlePos + (direction * middle);
        }

        /// <summary>
        /// Spawn door assets for each room. Doors also have a rotation
        /// </summary>
        protected void SpawnDoors()
        {
            _traverser.Traverse(room =>
            {
                if (room.LastRoom is null)
                {
                    return;
                }

                RoomBehaviour currentRoom = _traverser[room.Pos];
                RoomBehaviour previousRoom = _traverser[room.LastRoom.Pos];

                DoorBehaviour currentDoor = Instantiate(_data.Door, currentRoom.GetTransform(currentRoom.Layers.MiddleLayerIndex));
                DoorBehaviour previousDoor = Instantiate(_data.Door, previousRoom.GetTransform(previousRoom.Layers.MiddleLayerIndex));

                SetDoor(currentDoor, previousDoor, room.LastRoom.Pos - room.Pos, currentRoom);
                SetDoor(previousDoor, currentDoor, room.Pos - room.LastRoom.Pos, previousRoom);
            });
        }

        /// <summary>
        /// Sets necessary data to connect two doors
        /// </summary>
        /// <param name="door">first door</param>
        /// <param name="other">second door</param>
        /// <param name="direction">direction between doors</param>
        /// <param name="room">room of the first door</param>
        protected void SetDoor(DoorBehaviour door, DoorBehaviour other, Vector2Int direction, RoomBehaviour room)
        {
            Vector3 spawnDirection = Utils.GetVector3FromMatrixPos(direction, _data.CellSize);
            Vector3 movePoint = spawnDirection * (_data.CellCount / 2 - 1);

            movePoint += room.transform.position;
            door.transform.localPosition = spawnDirection * (_data.CellCount / 2);

            TileType[,] layer = room.Layers[room.CurrentLayer];
            LayerPosition layerPosition = new(GetLayerPosition(direction, layer) - direction, layer);

            door.Set(movePoint, other, room, layerPosition);
            door.LayerPosition = new(GetLayerPosition(direction, layer), layer)
            {
                TileType = TileType.Door,
            };

            door.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg));
        }

        /// <summary>
        /// Generates and spawns rooms, layers and doors
        /// </summary>
        protected void GenerateAndSpawnLevel()
        {
            SpawnRoomAssets();
            GenereteLayers();
            SetDoorPositions();
            SpawnLayers();
            SpawnDoors();

            _data.RoomBehaviourEvent.Value = _traverser.Start;
        }

        protected void ScanRooms()
        {
            _traverser.TraverseUnique(room =>
            {
                RoomBehaviour behaviour = _traverser[room.Pos];

                behaviour.Scan();
            });
        }
    }
}
