using Gameplay.Player;

namespace Gameplay
{
    public interface IInteractableEnter
    {
        public void OnInteract(PlayerController controller);
        
    }

    public interface IInteractableLeave
    {
        public void OnPlayerLeave(PlayerController controller);
    }
}
