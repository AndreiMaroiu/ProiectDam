using Events;
using Gameplay.Events;
using Gameplay.Generation;
using UnityEngine;

namespace Gameplay.Managers
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private RoomEvent _roomEvent;
        [SerializeField] private RoomBehaviourEvent _roomBehaviourEvent;
        [SerializeField] private IntEvent _layersNumberEvent;

        public void Start()
        {
            _roomBehaviourEvent.OnValueChanged += OnRoomChanged;
        }

        private void OnRoomChanged()
        {
            RoomBehaviour room = _roomBehaviourEvent;

            _roomEvent.Value = room.Room;
            _layersNumberEvent.Value = room.Layers.Count;
        }
    }
}
