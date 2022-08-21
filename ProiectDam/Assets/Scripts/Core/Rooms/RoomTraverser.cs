using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class RoomTraverser<T>
    {
        private readonly Dictionary<Vector2Int, T> _matrix;
        private readonly Room _start;

        public RoomTraverser(Room start)
        {
            _matrix = new Dictionary<Vector2Int, T>();
            _start = start;
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
            HashSet<Vector2Int> wasTraversed = new HashSet<Vector2Int>();

            queue.Enqueue(_start);

            while (queue.Count > 0)
            {
                Room top = queue.Dequeue();

                if (!wasTraversed.Contains(top.Pos))
                {
                    action(top);
                    wasTraversed.Add(top.Pos);
                }
                
                foreach (Room room in top)
                {
                    queue.Enqueue(room);
                }
            }
        }

        public T Start => this[_start.Pos];

        public T this[Vector2Int where]
        {
            get
            {
                if (!_matrix.ContainsKey(where))
                {
                    _matrix[where] = default;
                }
                return _matrix[where];
            }
            set => _matrix[where] = value;
        }
    }
}
