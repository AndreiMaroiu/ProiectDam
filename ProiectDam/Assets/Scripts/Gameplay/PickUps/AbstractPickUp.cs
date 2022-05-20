using Gameplay.Player;

namespace Gameplay.PickUps
{
    public abstract class AbstractPickUp : IInteractable
    {
        protected int _boost;

        public AbstractPickUp(int boost)
        {
            _boost = boost;
        }

        public abstract void Interact(PlayerController controller);

        void IInteractable.OnPlayerLeave(PlayerController controller)
        {
            
        }
    }
}
