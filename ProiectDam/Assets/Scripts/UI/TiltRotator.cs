using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class TiltRotator : MonoBehaviour
    {
        [SerializeField] private float _limit;
        [SerializeField] private float _speed;

        private RectTransform _rect;
        private Vector2 _startPos;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
            _startPos = _rect.anchoredPosition;

            Input.gyro.enabled = true;
        }

//#if PLATFORM_ANDROID

        private void Update()
        {
            Vector3 tilt = Input.gyro.rotationRate;
            Vector2 temp = new(tilt.y, tilt.x);
            Vector2 target = _rect.anchoredPosition + temp * Time.deltaTime * _speed;

            target.x = Math.Clamp(target.x, _startPos.x - _limit, _startPos.x + _limit);
            target.y = Math.Clamp(target.y, _startPos.y - _limit, _startPos.y + _limit);

            _rect.anchoredPosition = target;
        }

//#endif
    }
}
