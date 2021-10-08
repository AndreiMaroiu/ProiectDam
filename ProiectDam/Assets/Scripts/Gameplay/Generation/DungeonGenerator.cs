using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Generation
{
    internal class DungeonGenerator
    {
        private readonly int _maxRoomNeighbours;
        private readonly RoomType[,] _matrix;
        private readonly int _maxRoomCount;
        private readonly WeightedRandom<int> _weightedRandom;
        private readonly RandomPicker<int> _picker;

        public DungeonGenerator(int maxRoomNeighbours, int maxRoomCount, int matrixSize)
        {
            _maxRoomCount = maxRoomCount;
            _maxRoomNeighbours = maxRoomNeighbours;
            _matrix = CreateEmptyMatrix(matrixSize);

            _weightedRandom = new WeightedRandom<int>(new int[] { 1, 2, 3 }, new int[] { 60, 30, 10 });
            _picker = new RandomPicker<int>(new int[] { 0, 90, 270 }); // se considera mereu ca o camera vine din stanga
        }

        public RoomType[,] Matrix => _matrix;

        private static RoomType[,] CreateEmptyMatrix(int length)
        {
            RoomType[,] tempMatrix = new RoomType[length + 2, length + 2];

            for (int i = 0; i <= length + 1; i++)
            {
                tempMatrix[0, i] = RoomType.Wall;
                tempMatrix[i, 0] = RoomType.Wall;
                tempMatrix[length + 1, i] = RoomType.Wall;
                tempMatrix[i, length + 1] = RoomType.Wall;
            }

            for (int i = 1; i < length + 1; i++)
            {
                for (int j = 1; j < length + 1; j++)
                {
                    tempMatrix[i, j] = RoomType.Empty;
                }
            }

            return tempMatrix;
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

        private short TryAddToDirection(Room currentRoom, int dir)
        {
            Vector2 neighbour = Utils.GetDirectionRounded(dir);
            int x = (int)(currentRoom.Pos.x + neighbour.x);
            int y = (int)(currentRoom.Pos.y + neighbour.y);

            if (_matrix[x, y] == RoomType.Empty)
            {
                int numberOfNeighbours = GetNeighboursCount(currentRoom);

                if (numberOfNeighbours <= _maxRoomNeighbours)
                {
                    currentRoom.AddNeighbour(currentRoom.Pos + neighbour, dir, RoomType.Normal);
                    return 1;
                }
            }

            return 0;
        }

        private void CreateNeighbours(Room currentRoom, ref int numberOfRooms)
        {
            _picker.Reset();

            int remainingRooms = _maxRoomCount - numberOfRooms;
            int neighbourCount = _weightedRandom.RandomWeighted();
            neighbourCount = Mathf.Clamp(neighbourCount, 0, remainingRooms);

            for (int i = 0; i < neighbourCount; i++)
            {
                numberOfRooms += TryAddToDirection(currentRoom, _picker.Take() + currentRoom.Direction);
            }
        }

        private void CreateRoom(Room currentRoom, Room lastRoom, ref int numberOfRooms)
        {
            _matrix[(int)currentRoom.Pos.x, (int)currentRoom.Pos.y] = currentRoom.Type;
            CreateNeighbours(currentRoom, ref numberOfRooms);
        }

        public Room GenerateDungeon()
        {
            int numberOfRooms = 0;

            int middle = _matrix.GetLength(0) / 2;
            Vector2 startPosition = new Vector2(middle, middle);
            Room start = new Room(startPosition, null, RoomType.Start, Random.Range(0, 4) * 90);
            Queue<Room> queue = new Queue<Room>();

            queue.Enqueue(start);
            _matrix[middle, middle] = RoomType.Start;
            CreateNeighbours(start, ref numberOfRooms);

            while (queue.Count > 0)
            {
                Room top = queue.Dequeue();

                foreach (Room room in top)
                {
                    CreateRoom(room, top, ref numberOfRooms);
                    queue.Enqueue(room);
                }
            }

            return start;
        }
    }
}