using Gameplay.Generation;
using Gameplay.Player;
using System;

namespace Gameplay.Enemies
{
    public abstract class BaseEnemy : MovingObject
    {
        public event Action<BaseEnemy> OnDeathEvent;

        public abstract void OnEnemyTurn(PlayerController player);

        protected override void OnDeath()
        {
            OnDeathEvent?.Invoke(this);
            LayerPosition.Clear();
            Destroy(this.gameObject);
        }
    }
}
