using Gameplay.Player;

namespace Gameplay.PickUps
{
    public class MaxHealthEnergyBoost : AbstractPickUp
    {
        public MaxHealthEnergyBoost(int boost) : base(boost)
        {

        }

        protected override void Interact(PlayerController controller)
        {
            controller.MaxHealth *= _boost;
            controller.MaxEnergy *= _boost;
        }
    }
}
