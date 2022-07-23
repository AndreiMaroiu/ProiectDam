using Gameplay.Player;

namespace Gameplay.PickUps
{
    public class CoinPickUp : AbstractPickUp
    {
        public CoinPickUp(int coins) : base(coins)
        {

        }

        protected override void Interact(PlayerController controller)
        {
            // TODO: add coins to player
        }
    }
}
