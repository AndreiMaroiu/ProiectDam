using Core.Events;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public sealed class ChestHelper : MonoBehaviour, IInteractableEnter, IInteractableLeave
    {
        [SerializeField] private ButtonEvent _buttonEvent;
        private bool _canClick;
        private UnityAction onClick;
        private ButtonEvent.ButtonInfo _buttonInfo;

        public bool CanInteract { get; private set; }
        public PlayerController Controller { get; private set; }
        public UnityAction OnClick 
        {
            get => onClick; 
            set
            {
                onClick = value;
                _buttonInfo = new("Open chest", OnClick, true);
            }
        }


        public bool CanClick
        {
            get => _canClick;
            set
            {
                _canClick = value;

                if (value is false)
                {
                    _buttonEvent.Close(_buttonInfo);
                }
            }
        }

        public void OnInteract(PlayerController controller)
        {
            CanInteract = true;
            Controller = controller;

            if (CanClick)
            {
                _buttonEvent.Show(_buttonInfo);
            }
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            CanInteract = false;

            if (CanClick)
            {
                _buttonEvent.Close(_buttonInfo);
            }
        }
    }
}
