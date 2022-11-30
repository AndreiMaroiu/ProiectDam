using Gameplay.Generation;

namespace Gameplay
{
    public abstract class KillableObject : TileObject
    {
        public virtual int Priority { get; } = 1;
        public virtual int Health { get; set; }
        public virtual int MaxHealth { get; set; }
        public bool IsDead { get; private set; }
        public bool CanHit { get; protected set; } = true;

        public virtual int Score
        {
            get
            {
                if (IsDead)
                {
                    return 1;
                }

                return 0;
            }
        }

        public void InitHealth(int health)
        {
            MaxHealth = health;
            Health = health;
        }

        public void TakeDamage(int damage)
        {
            if (!CanHit || IsDead)
            {
                return;
            }

            Health -= damage;
            OnDamage();

            if (Health <= 0)
            {
                IsDead = true;
                OnDeath();
            }
        }

        protected abstract void OnDamage();
        protected abstract void OnDeath();
        public abstract void OnDeathFinished();
    }
}
