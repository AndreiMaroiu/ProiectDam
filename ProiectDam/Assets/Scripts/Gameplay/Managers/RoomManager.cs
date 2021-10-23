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
            _layersNumberEvent.Value = room.Layers.Count;
            _currentLayerEvent.Value = room.CurrentLayer;
        }

        private void OnLayerChanged()
        {
            _roomBehaviourEvent.Value.ChangedLayer(_currentLayerEvent.Value);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _currentLayerEvent.Value = 0;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentLayerEvent.Value = 1;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _currentLayerEvent.Value = 2;
            }
        }
    }
}
