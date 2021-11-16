using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseManager : MonoBehaviour
    {
        public float LastTimeScale { get; set; }

        public const float PausedScale = 0.0f;

        [SerializeField] private GameObject _pauseCanvas;

        void Awake()
        {
            LastTimeScale = Time.timeScale;
            _pauseCanvas.SetActive(false);
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

        public void OnCloseClick()
        {
            SceneManager.LoadScene(1);
        }
    }
}
