using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Values
{
    public sealed class HubPoint : MonoBehaviour
    {
        [SerializeField] private HubPoint[] _neighbours;

        private Dictionary<Vector2Int, HubPoint> _direction;

        public event Action OnEnter; // TODO: maybe removed this
        public event Action OnLeave;
        public void Activate(bool isActive)
        {
            if (isActive)
            {
                OnEnter?.Invoke();
            }
            else
            {
                OnLeave?.Invoke();
            }
        }

        private void Start()
        {
            _direction = new();

            foreach (var item in _neighbours)
            {
                Vector3 direction = item.transform.position - transform.position;
                direction = direction.normalized;

                Vector2Int key;

                if (direction.x == 0 || direction.y == 0)
                {
                    key = new Vector2Int((int)direction.x, (int)direction.y);
                }
                else
                {
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        key = new Vector2Int(direction.x.RoundToInt(), 0);
                    }
                    else
                    {
                        key = new Vector2Int(0, direction.y.RoundToInt());
                    }
                }

                _direction.Add(key, item);
            }
        }

        public bool CanMoveToDirection(Vector2Int direction)
        {
            return _direction.ContainsKey(direction);
        }

        public HubPoint GetPointInDirection(Vector2Int direction)
        {
            return _direction[direction];
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.5f);

            foreach (var item in _neighbours)
            {
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }
}
