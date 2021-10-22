using Gameplay.Events;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Generation
{
    public class DoorBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private RoomBehaviourEvent _event;

        public bool IsLocked { get; set; }

        private DoorBehaviour _other;
        private RoomBehaviour _room;
        private Vector3 _movePoint;

        public void Set(Vector3 movePoint, DoorBehaviour other, RoomBehaviour room)
        {
            _room = room;
            _other = other;
            _movePoint = movePoint;
        }

        private void Move(Transform transform)
        {
            transform.position = _other._movePoint;
            _event.Value = _other._room;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(_movePoint, new Vector3(1, 1, 1));
        }

        void IInteractable.Interact(PlayerController controller)
        {
            Move(controller.transform);
        }
    }
}
