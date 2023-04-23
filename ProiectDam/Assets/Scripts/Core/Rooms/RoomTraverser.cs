using Core.Values;
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
        /// Traverse each room in the graph only once
        /// </summary>
        public void TraverseUnique(Action<Room> action)
        {
            Queue<Room> queue = new();
            HashSet<Vector2IntPos> wasTraversed = new();

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
                    if (!wasTraversed.Contains(room.Pos))
                    {
                        queue.Enqueue(room);
                    }   
                }
            }
        }

        public void TraverseNeighboursUnique(Action<Room, Room> action)
        {
            HashSet<Vector2Tuple> connections = new();

            TraverseUnique(room =>
            {
                foreach (var neighbour in room)
                {
                    if (connections.Contains(new Vector2Tuple(neighbour.Pos, room.Pos)))
                    {
                        continue;
                    }

                    connections.Add(new Vector2Tuple(neighbour.Pos, room.Pos));

                    action(neighbour, room);
                }
            });
        }

        public T Start => this[_start.Pos];

        public Room GetStartRoom() => _start;

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
