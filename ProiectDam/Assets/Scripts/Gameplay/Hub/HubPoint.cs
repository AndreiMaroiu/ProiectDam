using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class HubPoint : MonoBehaviour
    {
        [SerializeField] private HubPoint[] _neightbours;

        private void Start()
        {
            // TODO: create dictionary with directions
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 0.2f);

            if (_neightbours.Length is 0)
            {
                return;
            }

            foreach (var neighbour in _neightbours)
            {
                if (neighbour == null)
                {
                    continue;
                        
                }

                Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }
    }
}
