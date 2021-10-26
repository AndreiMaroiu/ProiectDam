using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Gameplay.Generation
{
    internal class DungeonGenerator
    {
        public struct RoomDistance
        {
            public Room room;
            public int distance;

            public RoomDistance(Room room, int distance)
                => (this.room, this.distance) = (room, distance);
        }

        private readonly int _maxRoomNeighbours;
        private readonly RoomType[,] _matrix;
        private readonly int _maxRoomCount;
        private readonly List<RoomDistance> _distances;
        private readonly WeightedRandom<int> _weightedRandom;
        private readonly RandomPicker<int> _picker;

        private int _numberOfRooms = 0;
        private Room _start;

        public DungeonGenerator(int maxRoomNeighbours, int maxRoomCount, int matrixSize)
        {
            _maxRoomCount = maxRoomCount;
            _maxRoomNeighbours = maxRoomNeighbours;
            _matrix = new RoomType[matrixSize + 2, matrixSize + 2];

            _distances = new List<RoomDistance>();
            _weightedRandom = new WeightedRandom<int>(new int[] { 1, 2, 3 }, new int[] { 60, 30, 10 });
            _picker = new RandomPicker<int>(new int[] { 0, 90, 270 }); // se considera mereu ca o camera vine din stanga
        }

        public RoomType[,] Matrix => _matrix;
        public int NumberOfRooms => _numberOfRooms;
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
                Vector2 temp = Utils.GetDirectionRounded(i);
                int x = (int)(temp.x + currentRoom.Pos.x);
                int y = (int)(temp.y + currentRoom.Pos.y);

                if (_matrix[x, y] != RoomType.Empty)
                {
                    ++numberOfNeighbours;
                }
            }

            return numberOfNeighbours;
        }

        private byte TryAddToDirection(Room currentRoom, int dir)
        {
            Vector2Int neighbour = Utils.GetDirectionRounded(dir);
            int x = currentRoom.Pos.x + neighbour.x;
            int y = currentRoom.Pos.y + neighbour.y;

            if (_matrix[x, y] != RoomType.Empty)
            {
                return 0;
            }

            int numberOfNeighbours = GetNeighboursCount(currentRoom);

            if (numberOfNeighbours <= _maxRoomNeighbours)
            {
                currentRoom.AddNeighbour(currentRoom.Pos + neighbour, dir, RoomType.Normal);
                return 1;
            }

            return 0;
        }

        private void CreateNeighbours(Room currentRoom)
        {
            _picker.Reset();

            int remainingRooms = _maxRoomCount - _numberOfRooms;
            int neighbourCount = _weightedRandom.Take();
            neighbourCount = Mathf.Clamp(neighbourCount, 0, remainingRooms);

            for (int i = 0; i < neighbourCount; i++)
            {
                _numberOfRooms += TryAddToDirection(currentRoom, _picker.Take() + currentRoom.Direction);
            }
        }

        private void CreateRoom(Room currentRoom)
        {
            _matrix[(int)currentRoom.Pos.x, (int)currentRoom.Pos.y] = currentRoom.Type;
            CreateNeighbours(currentRoom);
        }

        public Room GenerateDungeon()
        {
            Reset();

            int middle = _matrix.GetLength(0) / 2;
            Vector2Int startPosition = new Vector2Int(middle, middle);
            _start = new Room(startPosition, null, RoomType.Start, Random.Range(0, 4) * 90);
            Queue<Room> queue = new Queue<Room>();

            queue.Enqueue(_start);
            _numberOfRooms = 0;

            while (queue.Count > 0)
            {
                Room top = queue.Dequeue();
                CreateRoom(top);

                foreach (Room room in top)
                {
                    queue.Enqueue(room);
                }
            }

            return _start;
        }

        public void CalculateDistances()
        {
            RoomTraverser<int> traverser = new RoomTraverser<int>(_start, _matrix.GetLength(0));

            traverser.Traverse(room =>
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

            _distances.Sort((a, b) => a.distance - b.distance);
        }

        private void Reset()
        {
            ClearMatrix();
            _numberOfRooms = 0;
            _picker.Reset();
            _distances.Clear();
        }
    }
}