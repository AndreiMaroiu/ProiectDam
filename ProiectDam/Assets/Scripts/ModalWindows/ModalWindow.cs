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

        private Action _closeAction = null;
        private Action _okAction = null;
        private Action _alternativeAction = null;

        private VerticalLayoutGroup group;

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

        private void SetWindow(string header, string content, Sprite image, string footer,
            Action okAction, Action closeAction, Action alternativeAction)
        {
            _window.gameObject.SetActive(true);

            _headerArea.SetActive(!string.IsNullOrEmpty(header));
            _headerText.text = header;

            _contentText.text = content;
            _contentImage.gameObject.SetActive(image != null);
            _contentImage.sprite = image;
            _contentImage.SetNativeSize();
            _contentArea.SetActive(!string.IsNullOrEmpty(content) || image != null);

            _footerArea.SetActive(!string.IsNullOrEmpty(footer));
            _footerText.text = footer;

            _okAction = okAction;
            _okButton.SetActive(okAction != null);

            _alternativeAction = alternativeAction;
            _alternativeButton.SetActive(alternativeAction != null);

            _closeAction = closeAction;
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
