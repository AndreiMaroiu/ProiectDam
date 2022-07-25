using Gameplay.Player;

namespace Gameplay
{
    public interface IInteractable
    {
        public void OnInteract(PlayerController controller);
        public void OnPlayerLeave(PlayerController controller);
    }
}
