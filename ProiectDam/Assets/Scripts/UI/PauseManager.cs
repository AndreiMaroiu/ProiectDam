using Core.DataSaving;
using Core.Events;
using ModalWindows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseCanvas;
        [SerializeField] private GameObject _howToCanvas;
        [SerializeField] private GameObject _optionsCanvas;
        [SerializeField] private Animator _transition;
        [SerializeField] private GameObject _saveButton;
        [SerializeField] private Color _modalColor;
        [Tooltip("Leave empty if no saving is needed")]
        [SerializeField] private SaveEvent _saveEvent;
        [SerializeField] private PanelStack _panelStack;

        void Awake()
        {
            _pauseCanvas.SetActive(false);
            _howToCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _saveButton.SetActive(_saveEvent != null);
            _panelStack.Clear();
        }

        public void OnPauseClick()
        {
            _panelStack.OpenDialog(_pauseCanvas, new PanelOptions()
            {
                CanClose = type => type is PanelType.Normal
            });
        }

        public void OnResumeClick()
        {
            _panelStack.ClosePanel();
        }

        public void OnOptionsClick()
        {
            _panelStack.OpenPanel(_optionsCanvas);
        }

        public void OnHowToClick()
        {
            _panelStack.OpenPanel(_howToCanvas);
        }

        public void OnBackClick()
        {
            _panelStack.ClosePanel();
        }

        public void OnRestartClick()
        {
            ModalWindow.Show(new ModalWindowData()
            {
                Header = "Are you sure you want to restart level?",
                OkText = "Restart",
                OkAction = () =>
                {
                    RestartLevel();
                    _saveEvent.Invoke(SaveType.SaveSeed);
                },
                AlternativeText = "Load new",
                AlternativeAction = () =>
                {
                    RestartLevel();
                    _saveEvent.Invoke(SaveType.DontSave);
                },
                IsTransparent = false,
                BackgroundColor = _modalColor,
            });
        }

        private void RestartLevel()
        {
            Time.timeScale = 1;
            _transition.SetTrigger("Start");

            SceneManager.LoadScene(Scenes.MainScene);
        }

        public void OnCloseClick()
        {
            ModalWindowData data = new()
            {
                Header = "Are you sure you want to leave?",
                OkText = "Leave",
                OkAction = LoadMainMenu,
                IsTransparent = false,
                BackgroundColor = _modalColor,
            };

            if (_saveEvent != null)
            {
                data.AlternativeText = "Save & Quit";
                data.AlternativeAction = () =>
                {
                    _saveEvent.Invoke(SaveType.Save);
                    LoadMainMenu();
                };
            }

            ModalWindow.ShowDialog(data);
        }

        private void LoadMainMenu()
        {
            Time.timeScale = 1; // TODO: maybe remove

            _panelStack.Clear();
            _transition.SetTrigger("Start");

            SceneManager.LoadScene(Scenes.MainMenu);
        }

        public void OnSaveClick()
        {
            _saveEvent.Invoke(SaveType.Save);

            ModalWindow.Show(new ModalWindowData()
            {
                Header = "Level saved successfully!",
                IsTransparent = false,
                BackgroundColor = _modalColor,
            });
        }
    }
}
