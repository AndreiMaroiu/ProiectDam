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

        protected abstract void Interact(PlayerController controller);

        public void OnInteract(PlayerController controller)
        {
            if (controller is null)
            {
                return;
            }

            Interact(controller);
        }

        void IInteractable.OnPlayerLeave(PlayerController controller)
        {
            
        }
    }
}
