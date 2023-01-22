using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace ModalWindows
{
    /// <summary>
    /// Modal Window, can be used to show short massages without the need to create more UI assets
    /// </summary>
    public partial class ModalWindow : MonoBehaviour
    {
        public static ModalWindow Instance { get; private set; } = null;

        private const float _transparentLevel = 0.5f;
        private const float _fullOpacityLevel = 1.0f;

        [SerializeField] private Canvas _window;
        [SerializeField] private bool _dontDestroyOnLoad;
        [SerializeField] private Image _windowBackground;

        [Header("Header")]
        [SerializeField] private GameObject _headerArea;
        [SerializeField] private Text _headerText;

        [Header("Content")]
        [SerializeField] private GameObject _contentArea;
        [SerializeField] private Text _contentText;
        [SerializeField] private Image _contentImage;

        [Header("Footer")]
        [SerializeField] private GameObject _footerArea;
        [SerializeField] private Text _footerText;

        [Header("Buttons")]
        [SerializeField] private GameObject _okButton;
        [SerializeField] private GameObject _alternativeButton;
        [SerializeField] private Text _closeText;
        [SerializeField] private Text _okText;
        [SerializeField] private Text _alternativeText;

        [Header("Utils")]
        [SerializeField] private PanelStack _panelStack;

        private UnityAction _closeAction = null;
        private UnityAction _okAction = null;
        private UnityAction _alternativeAction = null;

        private bool _canClose = true;

        private void TryClose()
        {
            if (_canClose is false)
            {
                return;
            }

            _panelStack.ClosePanel();
        }

        public void OnClose()
        {
            TryClose();
            _closeAction?.Invoke();
        }

        public void OnOk()
        {
            TryClose();
            _okAction?.Invoke();
        }

        public void OnAlternative()
        {
            TryClose();
            _alternativeAction?.Invoke();
        }

        private void SetWindow(IModalWindowData data, float? lastTimeScale = null)
        {
            _panelStack.OpenPanel(_window.gameObject, new PanelOptions() { PanelType = PanelType.Modal }, lastTimeScale);

            _headerArea.SetActive(!string.IsNullOrEmpty(data.Header));
            _headerText.text = data.Header;

            _contentText.text = data.Content;
            _contentImage.gameObject.SetActive(data.Image.IsNotNull());
            _contentImage.sprite = data.Image;
            _contentImage.SetNativeSize();
            _contentArea.SetActive(!string.IsNullOrEmpty(data.Content) || data.Image != null);

            _footerArea.SetActive(!string.IsNullOrEmpty(data.Footer));
            _footerText.text = data.Footer;

            _okAction = data.OkAction;
            _okButton.SetActive(data.OkAction != null && data.OkText != null);
            _okText.text = data.OkText;

            _alternativeAction = data.AlternativeAction;
            _alternativeButton.SetActive(data.AlternativeAction != null && data.AlternativeText != null);
            _alternativeText.text = data.AlternativeText;

            _closeAction = data.CloseAction;
            _closeText.text = data.CloseText;

            _canClose = data.CanClose;

            Color newColor = data.BackgroundColor;
            newColor.a = data.IsTransparent ? _transparentLevel : _fullOpacityLevel;
            _windowBackground.color = newColor;
        }

        private void Awake()
        {
            if (FindObjectsOfType<ModalWindow>().Length > 1)
            {
                DestroyImmediate(this.transform.parent.gameObject);
                return;
            }

            Instance = this;
            TryClose();

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
