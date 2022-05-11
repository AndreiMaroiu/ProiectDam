using System.Collections;
using UnityEngine;
using Utilities;
using Values;

namespace Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpikeBehaviour : MonoBehaviour
    {
        [SerializeField] private Sprite _activatedSprite;
        [SerializeField] private IntValue _damage;
        [Header("Animation")]
        [SerializeField] private float _animationTime = 0.5f;
        [SerializeField] private float _waitTime = 0.3f;

        private SpriteRenderer _renderer;
        private Sprite _original;
        private bool _canHit;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _original = _renderer.sprite;
            
        }

        private void OnEnable()
        {
            _canHit = false;
            StartCoroutine(WaitAfterEnable());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_canHit)
            {
                return;
            }

            KillableObject killableObject = collision.gameObject.GetComponent<KillableObject>();

            if (killableObject.IsNotNull())
            {
                killableObject.TakeDamage(_damage);
                StartCoroutine(StartAnimation());
            }
        }

        private IEnumerator StartAnimation()
        {
            yield return new WaitForSeconds(_waitTime);
            _renderer.sprite = _activatedSprite;
            yield return new WaitForSeconds(_animationTime);
            _renderer.sprite = _original;
        }

        private IEnumerator WaitAfterEnable()
        {
            yield return new WaitForSeconds(0.5f);
            _canHit = true;
        }
    }
}
