using Gameplay.Player;

namespace Gameplay.PickUps
{
    public class HealthBoost : AbstractPickUp
    {
        public HealthBoost(int boost) : base(boost)
        {

        }

        protected override void Interact(PlayerController controller)
        {
            controller.Health += _boost;
        }
    }
}
