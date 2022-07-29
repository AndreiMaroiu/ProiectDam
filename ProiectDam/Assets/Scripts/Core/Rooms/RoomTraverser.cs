using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class RoomTraverser<T>
    {
        private readonly T[,] _matrix;
        private readonly Room _start;
        private readonly int _matrixSize;

        public RoomTraverser(Room start, int matrixSize)
        {
            _matrixSize = matrixSize;
            _matrix = new T[matrixSize, matrixSize];
            _start = start;
        }

        public RoomTraverser(Room start, T[,] matrix)
        {
            _matrix = matrix;
            _start = start;
            _matrixSize = matrix.GetLength(0);
        }

        /// <summary>
        /// Traverse all rooms, including duplicates. Usefull for finding doors
        /// </summary>
        public void Traverse(Action<Room> action)
        {
            Queue<Room> queue = new Queue<Room>();

            queue.Enqueue(_start);

            while (queue.Count > 0)
            {
                Room top = queue.Dequeue();

                action(top);

                foreach (Room room in top)
                {
                    queue.Enqueue(room);
                }
            }
        }

        /// <summary>
        /// Traverse all rooms, excluding duplicates. Usefull for generating rooms
        /// </summary>
        public void TraverseUnique(Action<Room> action)
        {
            Queue<Room> queue = new Queue<Room>();
            bool[,] wasTraversed = new bool[_matrixSize, _matrixSize];

            queue.Enqueue(_start);

            while (queue.Count > 0)
            {
                Room top = queue.Dequeue();

                if (!wasTraversed[top.Pos.x, top.Pos.y])
                {
                    action(top);
                    wasTraversed[top.Pos.x, top.Pos.y] = true;
                }
                
                foreach (Room room in top)
                {
                    queue.Enqueue(room);
                }
            }
        }

        public void Traverse(Action<Vector2Int> action) 
            => Traverse(room => action(room.Pos));

        public void Traverse(Action<Vector2Int, T[,]> action) 
            => Traverse(room => action(room.Pos, _matrix));

        public T[,] Matrix => _matrix;

        public int Size => _matrix.GetLength(0);

        public T Start => this[_start.Pos];

        public ref T this[Vector2Int where] => ref _matrix[where.x, where.y];

        public ref T this[int x, int y] => ref _matrix[x, y];
    }
}
