using Core;
using Core.Values;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Gameplay.Generation
{
    internal class DungeonGenerator
    {
        public readonly struct RoomDistance
        {
            public readonly Room room;
            public readonly int distance;

            public RoomDistance(Room room, int distance)
                => (this.room, this.distance) = (room, distance);
        }

        private readonly int _maxRoomNeighbours;
        private readonly RoomType[,] _matrix;
        private readonly int _maxRoomCount;
        private readonly List<RoomDistance> _distances;
        private readonly WeightedRandom<int> _weightedRandom;
        private readonly RandomPicker<int> _picker;
        private readonly Dictionary<Vector2IntPos, Room> _rooms;

        private Room _start;

        public DungeonGenerator(int maxRoomNeighbours, int maxRoomCount, int matrixSize)
        {
            _maxRoomCount = maxRoomCount;
            _maxRoomNeighbours = maxRoomNeighbours;
            _matrix = new RoomType[matrixSize, matrixSize];

            _distances = new List<RoomDistance>();
            _weightedRandom = new WeightedRandom<int>(new int[] { 1, 2, 3 }, new int[] { 40, 30, 30 });
            _picker = new RandomPicker<int>(new int[] { 0, 90, 270 }); // se considera mereu ca o camera vine din stanga
            _rooms = new();
        }

        public RoomType[,] Matrix => _matrix;
        public int NumberOfRooms => _rooms.Count;
        public List<RoomDistance> Distances => _distances;

        private void ClearMatrix()
        {
            int length = _matrix.GetLength(0) - 1;

            for (int i = 0; i <= length; i++)
            {
                _matrix[0, i] = RoomType.Wall;
                _matrix[i, 0] = RoomType.Wall;
                _matrix[length, i] = RoomType.Wall;
                _matrix[i, length] = RoomType.Wall;
            }

            for (int i = 1; i < length; i++)
            {
                for (int j = 1; j < length; j++)
                {
                    _matrix[i, j] = RoomType.Empty;
                }
            }
        }

        private int GetNeighboursCount(Room currentRoom)
        {
            int numberOfNeighbours = 0;

            for (int i = 0; i < 360; i += 90)
            {
                Vector2Int temp = Utils.GetDirectionRounded(i);
                int x = temp.x + currentRoom.Pos.X;
                int y = temp.y + currentRoom.Pos.Y;

                if (_matrix[x, y] != RoomType.Empty)
                {
                    ++numberOfNeighbours;
                }
            }

            return numberOfNeighbours;
        }

        private void TryAddToDirection(Room currentRoom, int dir)
        {
            Vector2IntPos neighbour = Utils.GetDirectionRounded(dir);
            int x = currentRoom.Pos.X + neighbour.X;
            int y = currentRoom.Pos.Y + neighbour.Y;

            if (_matrix[x, y] == RoomType.Wall)
            {
                return;
            }

            int numberOfNeighbours = GetNeighboursCount(currentRoom);

            if (numberOfNeighbours <= _maxRoomNeighbours)
            {
                Vector2IntPos neighbourPos = currentRoom.Pos + neighbour;
                if (_rooms.TryGetValue(neighbourPos, out var oldNeighbour))
                {
                    currentRoom.AddNeighbour(oldNeighbour);
                    oldNeighbour.AddNeighbour(currentRoom);
                }
                else
                {
                    _rooms[neighbourPos] = currentRoom.AddNeighbour(neighbourPos, dir, RoomType.Normal);
                }
            }
        }

        private void CreateNeighbours(Room currentRoom)
        {
            _picker.Reset();

            int remainingRooms = _maxRoomCount - _rooms.Count;
            int neighbourCount = _weightedRandom.Take();
            neighbourCount = Mathf.Clamp(neighbourCount, 0, remainingRooms);

            for (int i = 0; i < neighbourCount; i++)
            {
                TryAddToDirection(currentRoom, _picker.Take() + currentRoom.Direction);
            }
        }

        private void CreateRoom(Room currentRoom)
        {
            _matrix[currentRoom.Pos.X, currentRoom.Pos.Y] = currentRoom.Type;
            CreateNeighbours(currentRoom);
        }

        public Room GenerateDungeon()
        {
            Reset();

            Queue<Room> queue = new();
            HashSet<Room> visited = new();
            int middle = _matrix.GetLength(0) / 2;
            Vector2Int startPosition = new(middle, middle);
            _start = new Room(startPosition, null, RoomType.Start, Random.Range(0, 4) * 90);
            _rooms[startPosition] = _start;

            queue.Enqueue(_start);
            int steps = 0;

            while (queue.Count > 0 && steps < 10000)
            {
                steps++;
                Room top = queue.Dequeue();

                if (!visited.Contains(top))
                {
                    visited.Add(top);
                }

                CreateRoom(top);

                foreach (Room room in top)
                {
                    if (!visited.Contains(room))
                    {
                        queue.Enqueue(room);
                    }
                }
            }

#if UNITY_EDITOR
            CheckForDuplicates();
#endif

            return _start;
        }

        public void CalculateDistances()
        {
            RoomTraverser<int> traverser = new(_start);

            traverser.TraverseUnique(room =>
            {
                if (room.LastRoom is null)
                {
                    traverser[room.Pos] = 0;
                }
                else
                {
                    traverser[room.Pos] = traverser[room.LastRoom.Pos] + 1;
                }

                _distances.Add(new RoomDistance(room, traverser[room.Pos]));
            });

            _distances.Sort((first, second) => first.distance - second.distance);
        }

        private Dictionary<Vector2Int, List<Room>> CalculateDuplicates()
        {
            Dictionary<Vector2Int, List<Room>> dict = new();
            Queue<Room> queue = new();
            HashSet<Room> wasTraversed = new();

            queue.Enqueue(_start);

            while (queue.Count > 0)
            {
                Room room = queue.Dequeue();

                if (!wasTraversed.Contains(room))
                {
                    if (!dict.ContainsKey(room.Pos))
                    {
                        dict.Add(room.Pos, new List<Room>());
                    }

                    dict[room.Pos].Add(room);
                    wasTraversed.Add(room);
                }

                foreach (Room neighbour in room)
                {
                    if (!wasTraversed.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                    }
                }
            }

            return dict;
        }

        private void CheckForDuplicates()
        {
            var duplicates = CalculateDuplicates();
            Debug.Log("Checking for duplicates");

            foreach (var (_, list) in duplicates)
            {
                if (list.Count > 1)
                {
                    Debug.LogError("Duplicate found!");
                }
            }
        }

        private void Reset()
        {
            ClearMatrix();
            _picker.Reset();
            _distances.Clear();
            _rooms.Clear();
        }
    }
}