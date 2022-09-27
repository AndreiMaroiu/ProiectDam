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
        private bool _canHit;
        private int? _turnCounter;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _original = _renderer.sprite;
            _playerTurn.OnValueChanged += OnTurnChange;
        }

        private void OnTurnChange()
        {
            if (!_turnCounter.HasValue)
            {
                return;
            }

            // spikes must wait 3 turn to activate again
            // 3 turn means:
            //      -> killable enters (attack)                     : 0
            //      -> killable turn ends                           : 1
            //      -> killable exits                               : 2
            //      -> killable turn ends                           : 3
            _turnCounter++;

            if (_turnCounter == 3) 
            {
                _canHit = true;
                _turnCounter = null;
            }
        }

        private void OnDestroy()
        {
            _playerTurn.OnValueChanged -= OnTurnChange;
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
                _turnCounter = 0;
                _canHit = false;
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
