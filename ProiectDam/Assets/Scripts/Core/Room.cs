using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Room
    {
        private readonly Vector2 _pos;
        private readonly int _direction;
        private readonly Room _lastRoom;
        private readonly List<Room> _neighbours;

        public RoomType Type { get; set; }
        public int Direction => _direction;
        public Vector2 Pos => _pos;

        public Room(Vector2 pos, Room lastRoom, RoomType type = RoomType.Empty, int direction = 0)
        {
            Type = type;
            _pos = pos;
            _direction = direction;

            _lastRoom = lastRoom;
            _neighbours = new List<Room>();
        }

        public Room(Vector2 pos, Room lastRoom, int direction)
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

        public void AddNeighbour(Vector2 offset, int angle, RoomType type = RoomType.Empty)
            => _neighbours.Add(new Room(offset, this, type, angle));

        public List<Room>.Enumerator GetEnumerator()
            => _neighbours.GetEnumerator();
    }
}