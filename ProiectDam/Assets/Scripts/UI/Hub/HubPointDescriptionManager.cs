using ModalWindows;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hub
{
    public class HubPointDescriptionManager : MonoBehaviour
    {
        [SerializeField] private HubPointDescriptionEvent _hubPointDescriptionEvent;
        [SerializeField] private Button _button;

        private void Start()
        {
            _hubPointDescriptionEvent.OnValueChanged += DescriptionChanged;

            _button.onClick.AddListener(OnClick);

            DescriptionChanged(_hubPointDescriptionEvent.Value);
        }

        private void DescriptionChanged(string description)
        {
            _button.interactable = !string.IsNullOrWhiteSpace(description);

        }

        private void OnDestroy()
        {
            _hubPointDescriptionEvent.OnValueChanged -= DescriptionChanged;
        }

        public void OnClick()
        {
            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Content = _hubPointDescriptionEvent.Value
            });
        }
    }
}
