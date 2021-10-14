using UnityEngine;

namespace Gameplay
{
    public class DoorBehaviour : MonoBehaviour
    {
        public bool IsLocked { get; set; }

        private DoorBehaviour _other;
        private Vector3 _movePoint;

        public void Set(Vector3 movePoint, DoorBehaviour other)
        {
            _other = other;
            _movePoint = movePoint;
        }
    }
}
