using Core.Events;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class InteractionHelper : MonoBehaviour, IInteractableEnter, IInteractableLeave
    {
        [SerializeField] private ButtonEvent _buttonEvent;
        private bool _canClick = true;

        public bool IsInteracting { get; private set; }
        public PlayerController Controller { get; private set; }
        public UnityAction OnClick { get; set; }
        public ButtonEvent.ButtonInfo ButtonInfo { get; set; }

        public void Set(UnityAction onClick, string buttonText, bool important = false)
        {
            OnClick = onClick;
            ButtonInfo = new(buttonText, onClick, important);
        }

        public bool CanClick
        {
            get => _canClick;
            set
            {
                _canClick = value;

                if (value is false)
                {
                    _buttonEvent.Close(ButtonInfo);
                }
            }
        }

        public void OnInteract(PlayerController controller)
        {
            IsInteracting = true;
            Controller = controller;

            if (CanClick)
            {
                _buttonEvent.Show(ButtonInfo);
            }
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            IsInteracting = false;

            if (CanClick)
            {
                _buttonEvent.Close(ButtonInfo);
            }
        }
    }
}
