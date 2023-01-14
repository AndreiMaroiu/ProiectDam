using UnityEngine;
using Utilities;

namespace UI.Map
{
    public class MapDisplayer : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] private GameObject _mapCanvas;
        [SerializeField] private GameObject _mainCanvas;
        [Header("Utils")]
        [SerializeField] private PanelStack _panelStack;

        private void Start()
        {
            _mapCanvas.SetActive(false);
        }

        public void OnMapClick()
        {
            _panelStack.OpenDialog(_mapCanvas);
            _mainCanvas.SetActive(false);
        }

        public void OnCloseClick()
        {
            _panelStack.ClosePanel();
            _mainCanvas.SetActive(true);
        }
    }
}
