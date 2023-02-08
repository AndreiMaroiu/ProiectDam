using Core.Events;
using Core.Mappers;
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
        public IButtonModel ButtonInfo { get; set; }

        public event UnityAction OnInteractionEnter;

        public void Set(IButtonModel model)
        {
            //OnClick = model.Action; // todo: update
            ButtonInfo = model;
        }

        public bool CanClick
        {
            get => _canClick;
            set
            {
                _canClick = value;

                if (value is false)
                {
                    _buttonEvent.Close(ButtonInfo.Owner); // TODO: refactor
                }
            }
        }

        public void OnInteract(PlayerController controller)
        {
            IsInteracting = true;
            Controller = controller;
            OnInteractionEnter?.Invoke();

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
                _buttonEvent.Close(ButtonInfo.Owner);
            }
        }
    }
}
