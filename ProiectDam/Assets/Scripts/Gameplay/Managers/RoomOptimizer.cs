using Core;
using Gameplay.Events;
using Gameplay.Generation;
using UnityEngine;
using Utilities;

namespace Gameplay.Managers
{
    public class RoomOptimizer : MonoBehaviour
    {
        [SerializeField] private RoomBehaviourEvent _roomEvent;

        private RoomBehaviour _lastRoom;

        private void Start()
        {
            Room start = _roomEvent.Value.Room;
            RoomTraverser<Room> traverser = new(start);

            traverser.TraverseUnique(room =>
            {
                if (room.GameObject.IsNotNull())
                {
                    room.GameObject.SetActive(false);
                }
            });

            SwitchActiveState(start, isActive: true);

            _lastRoom = _roomEvent.Value;
            _roomEvent.OnValueChanged += OnRoomChanged;

        }

        private void OnDestroy()
        {
            _roomEvent.OnValueChanged -= OnRoomChanged;
        }

        private void OnRoomChanged(RoomBehaviour room)
        {
            if (_lastRoom.IsNotNull())
            {
                SwitchActiveState(_lastRoom.Room, isActive: false);
            }

            SwitchActiveState(_roomEvent.Value.Room, isActive: true);

            _lastRoom = _roomEvent.Value;
        }

        private static void SwitchActiveState(Room room, bool isActive)
        {
            foreach (var neighour in room)
            {
                if (neighour.GameObject.IsNotNull())
                {
                    neighour.GameObject.SetActive(isActive);
                }
            }

            if (room.LastRoom != null && room.LastRoom.GameObject.IsNotNull())
            {
                room.LastRoom.GameObject.SetActive(isActive);
            }

            if (room.GameObject.IsNotNull())
            {
                room.GameObject.SetActive(isActive);
            }
        }
    }
}
