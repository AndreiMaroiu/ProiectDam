using Core.Events;
using Core.Values;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpikeBehaviour : MonoBehaviour
    {
        [SerializeField] private Sprite _activatedSprite;
        [SerializeField] private IntValue _damage;
        [SerializeField] private BoolEvent _playerTurn;
        [Header("Animation")]
        [SerializeField] private float _animationTime = 0.5f;
        [SerializeField] private float _waitTime = 0.3f;

        private SpriteRenderer _renderer;
        private Sprite _original;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _original = _renderer.sprite;
        }

        private void OnEnable()
        {
            StartCoroutine(WaitAfterEnable());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            KillableObject killableObject = collision.gameObject.GetComponent<KillableObject>();

            if (killableObject.IsNotNull())
            {
                if (killableObject is MovingObject moving)
                {
                    StartCoroutine(WaitToStopMovingAndHit(moving));
                }
                else
                {
                    DealDamage(killableObject);
                }
            }
        }

        private void DealDamage(KillableObject killable)
        {
            if (killable.CanHit is false)
            {
                return;
            }

            killable.TakeDamage(_damage, this);
            StartCoroutine(StartAnimation());
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
        }

        private IEnumerator WaitToStopMovingAndHit(MovingObject moving)
        {
            yield return new WaitWhile(() => moving.IsMoving);

            DealDamage(moving);
        }
    }
}
