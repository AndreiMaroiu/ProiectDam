using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Transform[] transformsToRotate;
        private Animator anim;
        private SpriteRenderer[] sr;

        private string WALK_ANIMATION = "Walk";

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sr = GetComponentsInChildren<SpriteRenderer>();
            transformsToRotate = GetComponentsInChildren<Transform>();
        }

        private void Update()
        {
            _direction.x = Input.GetAxis("Horizontal");
            _direction.y = Input.GetAxis("Vertical");
            AnimatePlayer();
           
        }
        //void PlayerMoveKeyboard()
        //{
        //    for (int i = 1; i < transformsToRotate.Length; i++)
        //    {
        //        transformsToRotate[i].position += new Vector3(_direction.x, 0f, 0f) * Time.deltaTime * _direction.x;
        //    }
            
        //}

        void AnimatePlayer()
        {
            if (_direction.x > 0)
            {
                anim.SetBool(WALK_ANIMATION, true);
                for (int i = 1; i < transformsToRotate.Length; i++)
                {
                    sr[i].flipX = false;
                }
                
            }
            else if (_direction.x < 0)
            {
                anim.SetBool(WALK_ANIMATION, true);
                for (int i = 1; i < transformsToRotate.Length; i++)
                {
                    sr[i].flipX = true;
                }
            }
            else if(_direction.y < 0 || _direction.y>0)
            { anim.SetBool(WALK_ANIMATION, true); }
            else
            {
                anim.SetBool(WALK_ANIMATION, false);
            }

        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + (_direction * (Time.fixedDeltaTime * _speed)));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

            interactable?.Interact(this);
        }
    }
}
