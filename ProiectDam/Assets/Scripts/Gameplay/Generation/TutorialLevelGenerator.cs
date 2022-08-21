using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Gameplay
{
    public class TutorialLevelGenerator
    {
        private RandomPicker<int> _picker;
        private HashSet<Vector2Int> _roomsPos;

        public TutorialLevelGenerator()
        {
            _picker = new RandomPicker<int>(new int[] { 0, 90, 270 });
            _roomsPos = new HashSet<Vector2Int>();
        }

        /// <summary>
        /// Generate a simple random dungeon based on a list of room types.
        /// Eg. Start -> Normal -> End
        /// </summary>
        /// <param name="roomTypes">list of types for the generated rooms</param>
        /// <returns></returns>
 	    public Room Generate(params RoomType[] roomTypes)
        {
            if (roomTypes is null || roomTypes.Length < 1)
            {
                Debug.LogError("Need at least on room type");
                return null;
            }

            Room first = GenerateRoom(roomTypes[0]);
            Room last = first;

            for (int i = 1; i < roomTypes.Length; i++)
            {
                last = GenerateRoom(roomTypes[i], last);
            }

            return first;
        }

        private Room GenerateRoom(RoomType type)
        {
            Vector2Int pos = Vector2Int.zero;
            int direction = Random.Range(0, 4) * 90;

            Room room = new Room(pos, null, type, direction);
            _roomsPos.Add(pos);

            return room;
        }

        private Room GenerateRoom(RoomType type, Room last)
        {
            int direction;
            Vector2Int pos;

            _picker.Reset();

            do
            {
                direction = _picker.Take();
                Vector2 offset = Utilities.Utils.GetDirection(direction);
                pos = last.Pos + new Vector2Int(Mathf.RoundToInt(offset.x), Mathf.RoundToInt(offset.y));
            } while (_roomsPos.Contains(pos) && _picker.CanTake);

            Room room = new Room(pos, last, type, direction);
            last.AddNeighbour(room);

            _roomsPos.Add(pos);

            return room;
        }
    }
}
