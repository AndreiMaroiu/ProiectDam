using Gameplay.Player;
using UnityEngine;
using Values;

namespace Gameplay.PickUps
{
    public class HealthPickUp : BasePickUp
    {
        [SerializeField] private IntValue _healthBonus;

        protected override void OnInteract(PlayerController controller)
        {
            controller.Health += _healthBonus;
        }
    }
}
