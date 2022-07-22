using Gameplay.Player;

namespace Gameplay
{
    public interface IInteractable
    {
        // TODO: add paramater for player controller
        public void OnInteract(PlayerController controller);
        public void OnPlayerLeave(PlayerController controller);
    }
}
