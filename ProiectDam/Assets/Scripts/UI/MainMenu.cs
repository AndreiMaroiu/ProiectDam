using Core.DataSaving;
using System.IO;
using UnityEngine;
using Utilities;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Canvases")]
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _howToCanvas;
        [SerializeField] private GameObject _optionsCanvas;
        [SerializeField] private GameObject _creditsCanvas;
        [SerializeField] private GameObject _statsCanvas;
        [SerializeField] private GameObject _savesCanvas;
        [Header("Utilities")]
        [SerializeField] private Animator _transition;
        [SerializeField] private LevelSaverHandler _saverHandler;
        [SerializeField] private AllSavesHandler _allSavesHandler;
        [SerializeField] private PanelStack _panelStack;

        private void Start()
        {
            _mainCanvas.SetActive(true);

            _howToCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _creditsCanvas.SetActive(false);
            _statsCanvas.SetActive(false);
            _savesCanvas.SetActive(false);

            _panelStack.Clear();

            _panelStack.OpenPanel(_mainCanvas, new PanelOptions()
            {
                PanelType = PanelType.Normal,
                CanClose = type => type is PanelType.Normal
            });
        }

        public void OnOptionsClick()
        {
            _panelStack.OpenPanel(_optionsCanvas);
        }

        public void OnCreditsClick()
        {
            _panelStack.OpenPanel(_creditsCanvas);
        }

        public void OnMenuClick()
        {
            _panelStack.ClosePanel();
        }

        public void OnHowToClick()
        {
            _panelStack.OpenPanel(_howToCanvas);
        }

        public void OnStartClick()
        {
            const string firstTimeKey = "FirstTime";
            _transition.SetTrigger("Start");
            StartCoroutine(Scenes.LoadAsync(Scenes.LoadingMenu));

            if (PlayerPrefs.GetInt(firstTimeKey, defaultValue: 0) == 1)
            {
                Loader.TargetScene = Scenes.MainScene;
                _saverHandler.SetForNewScene(_allSavesHandler.GetSaveFilePath(0));
            }
            else
            {
                Loader.TargetScene = Scenes.Tutorial;
                PlayerPrefs.SetInt(firstTimeKey, 1);
            }
        }

        public void OnStatsClick()
        {
            _panelStack.OpenPanel(_statsCanvas);
        }

        public void OnTutorialClick()
        {
            _transition.SetTrigger("Start");
            StartCoroutine(Scenes.LoadAsync(Scenes.LoadingMenu));
            Loader.TargetScene = Scenes.Tutorial;
        }

        /// <summary>
        /// View all saves panel
        /// </summary>
        public void OnLoadSavesClick()
        {
            _panelStack.OpenPanel(_savesCanvas);
        }

        /// <summary>
        /// Load a level based on a save file
        /// </summary>
        /// <param name="saveNumber">Number of the save file</param>
        public void OnLoadLevelClick(int saveNumber)
        {
            string saveFile = _allSavesHandler.GetSaveFilePath(saveNumber);

            if (!File.Exists(saveFile))
            {
                _saverHandler.SetForNewScene(saveFile);
            }
            else
            {
                _saverHandler.Load(saveFile);
            }

            _transition.SetTrigger("Start");
            StartCoroutine(Scenes.LoadAsync(Scenes.LoadingMenu));
            Loader.TargetScene = Scenes.MainScene;           
        }

        public void OnQuitClick()
        {
            Application.Quit();
        }
    }
}
