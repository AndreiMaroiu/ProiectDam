using Gameplay.Player;

namespace Gameplay
{
    public interface IInteractable
    {
        // TODO: add paramater for player controller
        public void Interact(PlayerController controller);
        public void OnPlayerLeave(PlayerController controller);
    }
}
