using Gameplay.Player;
using UnityEngine;
using Values;

namespace Gameplay.PickUps
{
    public class EnergyPickUp : BasePickUp
    {
        [SerializeField] private IntValue _energyBonus;

        protected override void OnInteract(PlayerController controller)
        {
            controller.Energy += _energyBonus;
        }
    }
}
