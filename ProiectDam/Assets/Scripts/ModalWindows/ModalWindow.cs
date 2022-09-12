using UnityEngine;
using UnityEngine.UI;
using Utilities;
using System;

namespace ModalWindows
{
    /// <summary>
    /// Modal Window, can be used to show short massages without the need to create more UI assets
    /// </summary>
    public partial class ModalWindow : MonoBehaviour
    {
        public static ModalWindow Instance { get; private set; } = null;

        private const int _transparentLevel = 81;
        private const int _fullOpacityLevel = 255;

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

        private Action _closeAction = null;
        private Action _okAction = null;
        private Action _alternativeAction = null;

        private float? _lastTimeScale = null;
        private bool _canClose = true;

        private void TryClose()
        {
            if (_canClose is false)
            {
                return;
            }

            _window.gameObject.SetActive(false);

            if (_lastTimeScale.HasValue)
            {
                Time.timeScale = _lastTimeScale.Value;
            }
        }

        public void OnClose()
        {
            _closeAction?.Invoke();
            TryClose();
        }

        public void OnOk()
        {
            _okAction?.Invoke();
            TryClose();
        }

        public void OnAlternative()
        {
            _alternativeAction?.Invoke();
            TryClose();
        }

        private void SetWindow(IModalWindowData data, float? lastTimeScale = null)
        {
            _lastTimeScale = lastTimeScale;
            _window.gameObject.SetActive(true);

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
