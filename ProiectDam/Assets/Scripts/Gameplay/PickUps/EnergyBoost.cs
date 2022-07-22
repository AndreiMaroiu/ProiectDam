using Gameplay.Player;

namespace Gameplay.PickUps
{
    public class EnergyBoost : AbstractPickUp
    {
        public EnergyBoost(int boost) : base(boost)
        {

        }

        protected override void Interact(PlayerController controller)
        {
            controller.Energy += _boost;
        }
    }
}
