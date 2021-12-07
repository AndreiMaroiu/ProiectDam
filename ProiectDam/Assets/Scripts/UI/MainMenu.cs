using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _howToCanvas;
        [SerializeField] private GameObject _optionsCanvas;
        [SerializeField] private GameObject _creditsCanvas;
        [SerializeField] private Animator _transition;

        void Awake()
        {
            _mainCanvas.SetActive(true);
            _howToCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _creditsCanvas.SetActive(false);
        }

        public void OnOptionsClick()
        {
            _optionsCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
        }

        public void OnCreditsClick()
        {
            _creditsCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
        }

        public void OnMenuClick()
        {
            _howToCanvas.SetActive(false);
            _creditsCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _mainCanvas.SetActive(true);
        }

        public void OnHowToClick()
        {
            _howToCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
        }

        public void OnStartClick()
        {
            _transition.SetTrigger("Start");
            SceneManager.LoadScene(Scenes.LoadingMenu);
        }

        public void OnQuitClick()
        {
            Application.Quit();
        }
    }
}
