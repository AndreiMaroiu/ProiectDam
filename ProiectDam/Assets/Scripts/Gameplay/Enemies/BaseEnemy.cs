using System;

namespace Gameplay.Enemies
{
    public abstract class BaseEnemy : KillableObject
    {
        public event Action<BaseEnemy> OnDeathEvent;

        protected override void OnDeath()
        {
            OnDeathEvent?.Invoke(this);
            LayerPosition.Clear();
            Destroy(this.gameObject);
        }
    }
}
