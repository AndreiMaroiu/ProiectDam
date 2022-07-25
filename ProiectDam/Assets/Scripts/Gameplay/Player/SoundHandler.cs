using UnityEngine;

namespace Gameplay.Player
{
    /// <summary>
    /// Player sound handler
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundHandler : MonoBehaviour
    {
        [SerializeField] private AudioClip _shootClip;
        [SerializeField] private AudioClip _walkClip;
        [SerializeField] private AudioClip _meleeClip;
        [SerializeField] private AudioClip _deathClip;

        private const float _randomRange = 0.2f;
        private AudioSource _currentSound;

        private void Start()
        {
            _currentSound = GetComponent<AudioSource>();
        }

        public void PlayMove() => PlaySound(_walkClip);

        public void PlayShoot() => PlaySound(_shootClip, _randomRange);

        public void PlayMelee() => PlaySound(_meleeClip, _randomRange);

        public void PlayDeath() => PlaySound(_deathClip);

        public void Stop() => _currentSound.Stop();

        private void PlaySound(AudioClip clip, float randomRange = 0.0f)
            => _currentSound.PlayOneShot(clip, _currentSound.volume + Random.Range(-randomRange, 0.0f));
    }
}
