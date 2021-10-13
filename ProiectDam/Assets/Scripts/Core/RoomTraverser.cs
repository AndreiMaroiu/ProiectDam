using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class RoomTraverser<T>
    {
        private readonly T[,] _matrix;
        private readonly Room _start;

        public RoomTraverser(Room start, int matrixSize)
        {
            _matrix = new T[matrixSize, matrixSize];
            _start = start;
        }

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

        public void Traverse(Action<Vector2Int> action) 
            => Traverse(room => action(room.Pos));

        public void Traverse(Action<Vector2Int, T[,]> action) 
            => Traverse(room => action(room.Pos, _matrix));

        public T[,] Matrix => _matrix;

        public ref T this[Vector2Int where] => ref _matrix[where.x, where.y];

        public ref T this[int x, int y] => ref _matrix[x, y];
    }
}
