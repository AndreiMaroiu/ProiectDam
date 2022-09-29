using Gameplay.Player;

namespace Gameplay.PickUps
{
    public class FreeEnergy : AbstractPickUp
    {
        public FreeEnergy(int boost) : base(boost)
        {

        }

        protected override void Interact(PlayerController controller)
        {
            controller.UseEnergyOnMove = false;
            controller.OnMoveStarted.OnEvent += OnMove;
            controller.AddPickUp(this);
        }

        private void OnMove(object sender)
        {
            _boost--;
            if (_boost is 0)
            {
                PlayerController controller = sender as PlayerController;

                controller.OnMoveStarted.OnEvent -= OnMove;
                controller.UseEnergyOnMove = true;
                controller.RemovePickup(this);
            }
        }
    }
}
