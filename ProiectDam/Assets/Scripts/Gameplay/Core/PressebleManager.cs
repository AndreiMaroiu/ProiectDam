using Core.Events;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class PressebleManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private ButtonEvent _event;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Time.timeScale == 0.0f)
            {
                return;
            }

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended)
                {
                    continue;
                }

                Ray ray = _camera.ScreenPointToRay(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000.0f, _layer);

                if (hit.collider.IsNull())
                {
                    continue;
                }

                PressableObject pressableObject = hit.collider.gameObject.GetComponent<PressableObject>();

                if (pressableObject.IsNotNull())
                {
                    _event.Press(hit.collider.gameObject);
                }
            }
        }
    }
}
