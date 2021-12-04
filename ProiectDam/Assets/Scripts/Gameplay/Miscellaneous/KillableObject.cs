using Gameplay.Generation;
using UnityEngine;
using System.Collections;

namespace Gameplay
{
    public abstract class KillableObject : TileObject
    {
        private bool _isDead = false;

        public virtual int Health { get; set; }
        public virtual int MaxHealth { get; set; }

        public void InitHealth(int health)
        {
            MaxHealth = health;
            Health = health;
        }

        public void TakeDamage(int damage)
        {
            if (_isDead)
            {
                return;
            }

            Health -= damage;
            OnDamage();

            if (Health <= 0)
            {
                OnDeath();
            }
        }

        protected abstract void OnDamage();
        protected abstract void OnDeath();
        public abstract void OnDeathFinished();
    }
}
