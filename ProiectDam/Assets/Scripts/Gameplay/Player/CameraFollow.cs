using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private RoomBehaviourEvent _roomEvent;
        [SerializeField] private float _smoothTime = 1;

        private Vector3 _velocity = Vector3.zero;

        private void Start()
        {
            Vector3 target = _roomEvent.Value.transform.position;
            transform.position = new Vector3(target.x, target.y, transform.position.z);
        }

        private void LateUpdate()
        {
            Vector3 position = _roomEvent.Value.transform.position;
            Vector3 target = new Vector3(position.x, position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, _smoothTime); ;
        }
    }
}
