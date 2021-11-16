using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PickUps
{
    public class EnergyPickUp : BasePickUp
    {
        [SerializeField] private int _energyBonus;

        protected override void OnInteract(PlayerController controller)
        {
            controller.Energy += _energyBonus;
        }
    }
}
