using Core.Events;
using UnityEngine;

namespace Gameplay.Hub
{
    public class HubCameraBehaviour : MonoBehaviour
    {
        [SerializeField] private HubPointEvent _target;
        [SerializeField] private float _smoothTime;

        private Vector3 _velocity = Vector3.zero;

        private void Start()
        {
            Debug.Log("camera start");
            transform.position = new(_target.Value.transform.position.x, _target.Value.transform.position.y, transform.position.z);
            // _target.Value.transform.position with { z = transform.position.z}; TODO: change for C# 10
        }

        private void LateUpdate()
        {
            Vector3 position = _target.Value.transform.position;
            Vector3 target = new(position.x, position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, _smoothTime);
        }
    }
}
