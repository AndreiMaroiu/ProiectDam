using UnityEngine;
using Utilities;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _howToCanvas;
        [SerializeField] private GameObject _optionsCanvas;
        [SerializeField] private GameObject _creditsCanvas;
        [SerializeField] private GameObject _statsCanvas;
        [SerializeField] private Animator _transition;

        private void Awake()
        {
            OnMenuClick();
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

        /// <summary>
        /// Close all other panels and activates main panel
        /// </summary>
        public void OnMenuClick()
        {
            _mainCanvas.SetActive(true);

            _howToCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _creditsCanvas.SetActive(false);
            _statsCanvas.SetActive(false);
        }

        public void OnHowToClick()
        {
            _howToCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
        }

        public void OnStartClick()
        {
            const string firstTimeKey = "FirstTime";
            _transition.SetTrigger("Start");
            StartCoroutine(Scenes.LoadAsync(Scenes.LoadingMenu));

            if (PlayerPrefs.GetInt(firstTimeKey, defaultValue: 0) == 1)
            {
                Loader.TargetScene = Scenes.MainScene;
            }
            else
            {
                Loader.TargetScene = Scenes.Tutorial;
                PlayerPrefs.SetInt(firstTimeKey, 1);
            }
        }

        public void OnStatsClick()
        {
            _mainCanvas.SetActive(false);
            _statsCanvas.SetActive(true);
        }

        public void OnTutorialClick()
        {
            _transition.SetTrigger("Start");
            StartCoroutine(Scenes.LoadAsync(Scenes.LoadingMenu));
            Loader.TargetScene = Scenes.Tutorial;
        }

        public void OnQuitClick()
        {
            Application.Quit();
        }
    }
}
