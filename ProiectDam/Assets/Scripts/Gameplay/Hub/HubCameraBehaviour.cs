using Core.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class HubCameraBehaviour : MonoBehaviour
    {
        // todo: target as hub

        [SerializeField] private Transform _target;
        [SerializeField] private float _smoothTime;

        private Vector3 _velocity = Vector3.zero;

        private void LateUpdate()
        {
            Vector3 position = _target.position;
            Vector3 target = new(position.x, position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, _smoothTime);
        }
    }
}
