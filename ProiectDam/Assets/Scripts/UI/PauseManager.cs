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
        private const float PausedScale = 0.0f;

        [SerializeField] private GameObject _pauseCanvas;
        [SerializeField] private GameObject _howToCanvas;
        [SerializeField] private GameObject _optionsCanvas;
        [SerializeField] private Animator _transition;
        [SerializeField] private GameObject _saveButton;
        [SerializeField] private Color _modalColor;
        [Tooltip("Leave empty if no saving is needed")]
        [SerializeField] private SaveEvent _saveEvent;

        private float _lastTimeScale;

        void Awake()
        {
            _pauseCanvas.SetActive(false);
            _howToCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _saveButton.SetActive(_saveEvent != null);
        }

        public void OnPauseClick()
        {
            _lastTimeScale = Time.timeScale;
            Time.timeScale = PausedScale;
            _pauseCanvas.SetActive(true);
        }

        public void OnResumeClick()
        {
            Time.timeScale = _lastTimeScale;
            _pauseCanvas.SetActive(false);
        }

        public void OnOptionsClick()
        {
            _optionsCanvas.SetActive(true);
            _pauseCanvas.SetActive(false);
        }

        public void OnHowToClick()
        {
            _howToCanvas.SetActive(true);
            _pauseCanvas.SetActive(false);
        }

        public void OnBackClick()
        {
            _howToCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _pauseCanvas.SetActive(true);
        }

        public void OnRestartClick()
        {
            ModalWindows.ModalWindow.Show(new ModalWindows.ModalWindowData()
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

            ModalWindow.Show(data);
        }

        private void LoadMainMenu()
        {
            Time.timeScale = 1;
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
