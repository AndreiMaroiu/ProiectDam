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
        [SerializeField] private GameObject _optionsCanvas;

        void Awake()
        {
            LastTimeScale = Time.timeScale;
            _pauseCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
        }

        public void OnPauseClick()
        {
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

        public void OnBackClick()
        {
            _optionsCanvas.SetActive(false);
            _pauseCanvas.SetActive(true);
        }

        public void OnCloseClick()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(Scenes.MainMenu);
        }
    }
}
