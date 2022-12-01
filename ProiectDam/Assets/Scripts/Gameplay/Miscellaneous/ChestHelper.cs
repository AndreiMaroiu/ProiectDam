using Core.Events;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class ChestHelper : MonoBehaviour, IInteractableEnter, IInteractableLeave
    {
        [SerializeField] private ButtonEvent _buttonEvent;
        private bool _canClick;

        public bool CanInteract { get; private set; }
        public PlayerController Controller { get; private set; }
        public UnityAction OnClick { get; set; }

        public bool CanClick
        {
            get => _canClick;
            set
            {
                _canClick = value;

                if (value is false)
                {
                    _buttonEvent.Close("Open chest", OnClick);
                }
            }
        }

        public void OnInteract(PlayerController controller)
        {
            CanInteract = true;
            Controller = controller;

            if (CanClick)
            {
                _buttonEvent.Show("Open chest", OnClick);
            }
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            CanInteract = false;

            if (CanClick)
            {
                _buttonEvent.Close("Open chest", OnClick);
            }
        }
    }
}
