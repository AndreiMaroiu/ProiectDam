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
        [SerializeField] private IntEvent _currentLayerEvent;

        public void Start()
        {
            _roomBehaviourEvent.OnValueChanged += OnRoomChanged;
            _currentLayerEvent.OnValueChanged += OnLayerChanged;
        }

        private void OnRoomChanged()
        {
            RoomBehaviour room = _roomBehaviourEvent;

            _roomEvent.Value = room.Room;
            _currentLayerEvent.Value = room.CurrentLayer;
            _layersNumberEvent.Value = room.Layers.Count;
        }

        private void OnLayerChanged()
        {
            _roomBehaviourEvent.Value.ChangedLayer(_currentLayerEvent.Value);
        }
    }
}
