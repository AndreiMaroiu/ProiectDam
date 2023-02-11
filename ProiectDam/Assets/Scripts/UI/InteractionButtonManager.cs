using Core.Events;
using Core.Events.Binding;
using Core.Mappers;
using System.Collections.Generic;
using UI.ButtonViews;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class InteractionButtonManager : BaseBindableBehaviour
    {
        [SerializeField] private Button _middleButton;
        [SerializeField] private Text _middleButtonText;
        [SerializeField] private Image _interactionIcon;
        [SerializeField] private ButtonEvent _buttonEvent;

        private List<IButtonModel> _buttonInfos;
        private ButtonMapper _buttonMapper;

        private void Start()
        {
            Bind(_buttonEvent.OnShowBindable, OnShowButton);
            Bind(_buttonEvent.OnCloseBindable, OnCloseButton);
            Bind(_buttonEvent.OnPressBindable, OnObjectPressed);

            _buttonInfos = new();
            _buttonMapper = new();
            _buttonMapper.Map(typeof(SimpleButtonModel), new SimpleView());
            _buttonMapper.Map(typeof(GolderChaliceModel), new GoldenChaliceView());
            _buttonMapper.TargetButton = new()
            {
                Button = _middleButton,
                Text = _middleButtonText,
                Icon = _interactionIcon,
            };

            _middleButton.gameObject.SetActive(false);
        }

        private void OnShowButton(IButtonModel info)
        {
            _buttonMapper.Setup(info);
            _buttonInfos.Add(info);

            _middleButton.gameObject.SetActive(true);
        }

        private void OnCloseButton(GameObject sender)
        {
            _middleButton.onClick.RemoveAllListeners();
            _buttonInfos.RemoveAll(x => x.Owner == sender);

            if (_buttonInfos.Count == 0)
            {
                _middleButton.gameObject.SetActive(false);
            }
            else
            {
                var peek = _buttonInfos[0];

                _buttonMapper.Setup(peek);
            }
        }

        private void OnObjectPressed(GameObject sender)
        {
            foreach (var model in _buttonInfos)
            {
                if (model.Owner == sender)
                {
                    OnShowButton(model);
                    _middleButton.onClick.Invoke();
                    break;
                }
            }
        }
    }
}
