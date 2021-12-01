using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
    public class SoundHandler : MonoBehaviour
    {
        private AudioSource _currentSound;
        [SerializeField] private AudioClip _shootClip;
        [SerializeField] private AudioClip _walkClip;
        [SerializeField] private AudioClip _meleeClip;
        [SerializeField] private AudioClip _deathClip;
        private void Start()
        {
           _currentSound = GetComponent<AudioSource>();

        }

        public void PlayMove() 
        {
            _currentSound.clip = _walkClip;
            _currentSound.Play();
        }

        public void PlayShoot()
        {
            _currentSound.clip = _shootClip;
            _currentSound.Play();
        }

        public void PlayMelee()
        {
            _currentSound.clip = _meleeClip;
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
