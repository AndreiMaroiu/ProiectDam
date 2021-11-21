using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MapDisplayer : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] private GameObject _mapCanvas;

        public float LastTimeScale { get; set; }

        public const float StoppedScale = 0.0f;

        void Awake()
        {
            LastTimeScale = Time.timeScale;
            _mapCanvas.SetActive(false);
        }

        public void OnMapClick()
        {
            Time.timeScale = StoppedScale;
            _mapCanvas.SetActive(true);
        }

        public void OnCloseClick()
        {
            Time.timeScale = LastTimeScale;
            _mapCanvas.SetActive(false);
        }
    }
}
