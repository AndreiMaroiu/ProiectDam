using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{
    public class TiltRotator : MonoBehaviour
    {
        [SerializeField] private float _limit;
        [SerializeField] private float _speed;

        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.position;

            Input.gyro.enabled = true;
        }

//#if PLATFORM_ANDROID

        private void Update()
        {
            Vector3 tilt = Input.gyro.rotationRate;
            Vector3 temp = new(tilt.y, tilt.x);
            Vector3 target = transform.position + _speed * Time.deltaTime * temp;

            target.x = Math.Clamp(target.x, _startPos.x - _limit, _startPos.x + _limit);
            target.y = Math.Clamp(target.y, _startPos.y - _limit, _startPos.y + _limit);

            transform.position = target;
        }

//#endif
    }
}
