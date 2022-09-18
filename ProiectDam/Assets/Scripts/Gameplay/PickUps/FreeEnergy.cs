using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.PickUps
{
    public class FreeEnergy : AbstractPickUp
    {
        private PlayerController _playerController;

        public FreeEnergy(int boost) : base(boost)
        {
        }

        protected override void Interact(PlayerController controller)
        {
            controller.UseEnergyOnMove = false;
            _playerController = controller;
            controller.OnMoveStarted.OnEvent += OnMove;
        }

        private void OnMove(object _)
        {
            _boost--;
            if (_boost is 0)
            {
                _playerController.UseEnergyOnMove = true;
            }
        }
    }
}
