using UnityEngine;

namespace Gameplay.Enemies
{
    public sealed class SimpleEnemy : BaseEnemy
    {
        [SerializeField] private int _startHealth = 1;

        private void Start()
        {
            MaxHealth = _startHealth;
            Health = _startHealth;
        }

        protected override void OnDamage()
        {
            // play sound and animations
        }
    }
}
