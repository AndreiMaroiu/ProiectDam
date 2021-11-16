using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PickUps
{
    public class HealthPickUp : BasePickUp
    {
        [SerializeField] private int _healthBonus;

        protected override void OnInteract(PlayerController controller)
        {
            controller.Health += _healthBonus;
        }
    }
}
