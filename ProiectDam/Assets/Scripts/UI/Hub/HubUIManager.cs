using UnityEngine;
using Utilities;

namespace UI
{
    public class HubUIManager : MonoBehaviour
    {
        [Header("Canvases")]
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _pausePanel;
        [Header("Utils")]
        [SerializeField] private PanelStack _panelStack;

        private void Start()
        {
            _mainPanel.SetActive(false);
            _pausePanel.SetActive(false);

            _panelStack.Clear();

            _panelStack.OpenPanel(_mainPanel);
        }

        public void OnPause()
        {
            _panelStack.OpenDialog(_pausePanel);
        }
    }
}
