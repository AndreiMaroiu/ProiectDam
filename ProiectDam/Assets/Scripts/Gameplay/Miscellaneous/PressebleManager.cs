using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class PressebleManager : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended)
                {
                    continue;
                }

                Ray ray = _camera.ScreenPointToRay(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider.IsNull())
                {
                    continue;
                }

                PressebleObject pressebleObject = hit.collider.gameObject.GetComponent<PressebleObject>();

                if (pressebleObject.IsNotNull())
                {
                    pressebleObject.OnClick();
                }
            }
        }
    }
}
