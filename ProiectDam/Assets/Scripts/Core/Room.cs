using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Room
    {
        private readonly Vector2Int _pos;
        private readonly int _direction;
        private readonly Room _lastRoom;
        private readonly List<Room> _neighbours;

        public RoomType Type { get; set; }
        public int Direction => _direction;
        public Vector2Int Pos => _pos;

        public Room(Vector2Int pos, Room lastRoom, RoomType type = RoomType.Empty, int direction = 0)
        {
            Type = type;
            _pos = pos;
            _direction = direction;

            _lastRoom = lastRoom;
            _neighbours = new List<Room>();
        }

        public Room(Vector2Int pos, Room lastRoom, int direction)
        {
            _direction = direction;
            _pos = pos;

            _lastRoom = lastRoom;
            _neighbours = new List<Room>();
        }

        public GameObject GameObject { get; set; }

        public List<Room> Neighbours => _neighbours;

        public Room LastRoom => _lastRoom;

        public void AddNeighbour(Room neighbour)
            => _neighbours.Add(neighbour);

        public void AddNeighbour(Vector2Int offset, int angle, RoomType type = RoomType.Empty)
            => _neighbours.Add(new Room(offset, this, type, angle));

        public List<Room>.Enumerator GetEnumerator()
            => _neighbours.GetEnumerator();

        public override string ToString()
        {
            string str = $"Room: dir({_direction.ToString()}), neighbours({_neighbours.Count.ToString()})\n";

            foreach (Room room in _neighbours)
            {
                str += $"Neighbour: dir({room._direction.ToString()}), neighbours({room._neighbours.Count.ToString()})\n";
            }

            return str;
        }
    }
}