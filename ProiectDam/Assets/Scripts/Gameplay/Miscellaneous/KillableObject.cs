using UnityEngine;
using Gameplay.Generation;

namespace Gameplay
{
    public abstract class KillableObject : TileObject
    {
        public virtual int Health { get; set; }
        public virtual int MaxHealth { get; set; }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health > 0)
            {
                OnDamage();
                return;
            }

            OnDeath();
        }

        protected abstract void OnDamage();

        protected abstract void OnDeath();
    }
}
