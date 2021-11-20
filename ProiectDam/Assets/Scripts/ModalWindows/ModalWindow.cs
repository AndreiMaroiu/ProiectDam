using Action = System.Action;
using UnityEngine;
using UnityEngine.UI;

namespace ModalWindows
{
    public partial class ModalWindow : MonoBehaviour
    {
        public static ModalWindow Instance { get; private set; } = null;

        [SerializeField] private Canvas _window;
        [SerializeField] private bool _dontDestroyOnLoad;

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

        private void Close()
        {
            _window.gameObject.SetActive(false);
        }

        public void OnClose()
        {
            _closeAction?.Invoke();
            Close();
        }

        public void OnOk()
        {
            _okAction?.Invoke();
            Close();
        }

        public void OnAlternative()
        {
            _alternativeAction?.Invoke();
            Close();
        }

        private void SetWindow(ModalWindowData data)
        {
            _window.gameObject.SetActive(true);

            _headerArea.SetActive(!string.IsNullOrEmpty(data.Header));
            _headerText.text = data.Header;

            _contentText.text = data.Content;
            _contentImage.gameObject.SetActive(data.Image != null);
            _contentImage.sprite = data.Image;
            _contentImage.SetNativeSize();
            _contentArea.SetActive(!string.IsNullOrEmpty(data.Content) || data.Image != null);

            _footerArea.SetActive(!string.IsNullOrEmpty(data.Footer));
            _footerText.text = data.Footer;

            _okAction = data.OkAction;
            _okButton.SetActive(data.OkAction != null);
            _okText.text = data.OkText;

            _alternativeAction = data.AlternativeAction;
            _alternativeButton.SetActive(data.AlternativeAction != null);
            _alternativeText.text = data.AlternativeText;

            _closeAction = data.CloseAction;
            _closeText.text = data.CloseText;
        }

        private void Awake()
        {
            if (FindObjectsOfType<ModalWindow>().Length > 1)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            Close();

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
