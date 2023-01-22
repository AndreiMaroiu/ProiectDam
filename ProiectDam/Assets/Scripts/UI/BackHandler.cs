using ModalWindows;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace UI
{
    public class BackHandler : MonoBehaviour
    {
        [SerializeField] private PanelStack _panelStack;
        [SerializeField] private int _min;
        [SerializeField] private UnityEvent _closeEvent;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_panelStack.PanelsCount > _min)
                {
                    _panelStack.ClosePanel();
                }
                else
                {
                    CloseGame();
                }
            }
        }

        private void CloseGame()
        {
            if (_closeEvent.GetPersistentEventCount() == 0)
            {
                ModalWindow.Show(new ModalWindowData()
                {
                    Header = "Are you sure you want to leave?",
                    OkText = "Leave Game",
                    OkAction = () => Application.Quit()
                });
            }
            else
            {
                _closeEvent.Invoke();
            }
        }
    }
}
