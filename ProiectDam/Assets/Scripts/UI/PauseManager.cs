using System.Collections;
using System.Collections.Generic;
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

        void Awake()
        {
            _pauseCanvas.SetActive(false);
            _howToCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
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

        public void OnCloseClick()
        {
            Time.timeScale = 1;
            _transition.SetTrigger("Start");
            SceneManager.LoadScene(Scenes.MainMenu);
        }
    }
}
