using System;
using UnityEngine;

namespace Utilities
{
    public class SwipeDetector
    {
        private const float SwipeThreshold = 20f;

        private Vector2 _fingerDown;
        private Vector2 _fingerUp;

        private bool _touchStarted = false;

        public event Action<Vector2Int> OnSwipe;

        public void CkeckForSwipes()
        {
            if (Time.timeScale == 0.0f)
            {
                _touchStarted = false;
                return;
            }

#if UNITY_EDITOR || !PLATFORM_ANDROID
            Vector2Int direction = GetMoveDirection();

            if (direction != Vector2Int.zero)
            {
                OnSwipe?.Invoke(direction);
            }
#endif

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _fingerUp = touch.position;
                        _fingerDown = touch.position;
                        _touchStarted = true;
                        Debug.Log("Touch stared!");
                        break;
                    case TouchPhase.Ended:
                        _fingerDown = touch.position;
                        CheckSwipe();
                        Debug.Log("Touch Ended!");
                        break;
                }
            }
        }

        private Vector2Int GetMoveDirection()
        {
            Vector2Int dir = Vector2Int.zero;

            if (Input.GetKeyDown(KeyCode.A))
            {
                dir.x = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                dir.x = 1;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                dir.y = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                dir.y = -1;
            }

            return dir;
        }

        void CheckSwipe()
        {
            if (!_touchStarted)
            {
                return;
            }

            //Check if Vertical swipe
            if (VerticalMove() > SwipeThreshold && VerticalMove() > HorizontalValMove())
            {
                if (_fingerDown.y - _fingerUp.y > 0) //up swipe
                {
                    OnSwipe?.Invoke(Vector2Int.up);
                }
                else if (_fingerDown.y - _fingerUp.y < 0) //Down swipe
                {
                    OnSwipe?.Invoke(Vector2Int.down);
                }

                _fingerUp = _fingerDown;
            }
            //Check if Horizontal swipe
            else if (HorizontalValMove() > SwipeThreshold && HorizontalValMove() > VerticalMove())
            {
                if (_fingerDown.x - _fingerUp.x > 0) //Right swipe
                {
                    OnSwipe?.Invoke(Vector2Int.right);
                }
                else if (_fingerDown.x - _fingerUp.x < 0) //Left swipe
                {
                    OnSwipe?.Invoke(Vector2Int.left);
                }

                _fingerUp = _fingerDown;
            }
        }

        float VerticalMove() => Mathf.Abs(_fingerDown.y - _fingerUp.y);

        float HorizontalValMove() => Mathf.Abs(_fingerDown.x - _fingerUp.x);
    }
}
