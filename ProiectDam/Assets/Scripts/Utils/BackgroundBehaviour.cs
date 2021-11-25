using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundBehaviour : MonoBehaviour
    {
        [SerializeField] private float _startSize;

        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();

            FitToScreen();
        }

        private void FitToScreen()
        {
            float height = _renderer.bounds.size.y;
            float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
            float size = worldScreenHeight / height * _startSize;

            transform.localScale = new Vector3(size, size);
        }
    }
}
