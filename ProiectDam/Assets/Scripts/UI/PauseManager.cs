using Core.DataSaving;
using ModalWindows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class PauseManager : MonoBehaviour
    {
        public float LastTimeScale { get; set; }

        public const float PausedScale = 0.0f;

        [SerializeField] private GameObject _pauseCanvas;
        [SerializeField] private GameObject _howToCanvas;
        [SerializeField] private GameObject _optionsCanvas;
        [SerializeField] private Animator _transition;
        [SerializeField] private GameObject _saveButton;
        [SerializeField] private LevelSaverManager _saver;
        [SerializeField] private Color _modalColor;

        void Awake()
        {
            _pauseCanvas.SetActive(false);
            _howToCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _saveButton.SetActive(_saver != null);
        }

        public void OnPauseClick()
        {
            LastTimeScale = Time.timeScale;
            Time.timeScale = PausedScale;
            _pauseCanvas.SetActive(true);
        }

        public void OnResumeClick()
        {
            Time.timeScale = LastTimeScale;
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
            _pauseCanvas.SetActive(false);

            ModalWindows.ModalWindow.Show(new ModalWindows.ModalWindowData()
            {
                Header = "Are you sure you want to restart level?",
                CloseAction = () => _pauseCanvas.SetActive(true),
                OkText = "Restart",
                OkAction = () =>
                {
                    RestartLevel();
                    _saver.SaveOnlySeed();
                },
                AlternativeText = "Load new",
                AlternativeAction = () =>
                {
                    RestartLevel();
                    _saver.SetUpForNewScene();
                }
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
            Time.timeScale = 1;
            _transition.SetTrigger("Start");

            SceneManager.LoadScene(Scenes.MainMenu);
        }

        public void OnSaveClick()
        {
            _saver.Save();

            ModalWindow.Show(new ModalWindowData()
            {
                Header = "Level saved successfully!",
                IsTransparent = false,
                BackgroundColor = _modalColor,
            });
        }
    }
}
