using UnityEngine;

namespace Gameplay
{

    public class Levitator : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private float _speed;

        private float _initialY;

        private void Start()
        {
            _initialY = transform.position.y;
        }

        private void Update()
        {
            transform.position = new(transform.position.x, _initialY + (Mathf.Sin(Time.time * _speed) * _distance), transform.position.z);
        }
    }
}
