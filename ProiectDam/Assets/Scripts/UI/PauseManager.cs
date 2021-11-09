using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PauseManager : MonoBehaviour
    {
        public bool IsPaused { get; set; }
        public float LastTimeScale { get; set; }

        public const float PausedScale = 0.0f;

        [SerializeField] private GameObject _pauseCanvas;

        void Awake()
        {
            LastTimeScale = Time.timeScale;
            _pauseCanvas.SetActive(false);
        }
        public void onPauseClick()
        {
            Time.timeScale = PausedScale;
            IsPaused = !IsPaused;
            _pauseCanvas.SetActive(IsPaused);
        }
        public void onResumeClick()
        {
            Time.timeScale = LastTimeScale;
            IsPaused = !IsPaused;
            _pauseCanvas.SetActive(IsPaused);
        }
    }
}
