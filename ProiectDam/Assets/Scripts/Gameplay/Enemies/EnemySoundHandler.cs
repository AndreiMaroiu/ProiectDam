using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemySoundHandler : MonoBehaviour
    {
        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private AudioClip _hitClip;
        [SerializeField] private AudioClip _deathClip;
        [SerializeField] private AudioClip _walkClip;

        private AudioSource _currentSound;

        private void Start()
        {
            _currentSound = GetComponent<AudioSource>();
        }

        public void PlayAttack()
        {
            _currentSound.clip = _attackClip;
            _currentSound.Play();
        }

        public void PlayHit()
        {
            _currentSound.clip = _hitClip;
            _currentSound.Play();
        }

        public void PlayWalk()
        {
            _currentSound.clip = _walkClip;
            _currentSound.Play();
        }

        public void PlayDeath()
        {
            _currentSound.clip = _deathClip;
            _currentSound.Play();
        }

        public void Stop() => _currentSound.Stop();
    }
}
