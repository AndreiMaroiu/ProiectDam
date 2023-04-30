using Core;
using Core.DataSaving;
using Core.Services;
using ModalWindows;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private const string _savePath = "SavePath";

        [Header("Canvases")]
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _howToCanvas;
        [SerializeField] private GameObject _optionsCanvas;
        [SerializeField] private GameObject _creditsCanvas;
        [SerializeField] private GameObject _statsCanvas;
        [SerializeField] private GameObject _savesCanvas;
        [SerializeField] private GameObject _savesPanel;
        [Header("UI elements")]
        [SerializeField] private Button _continueButton;
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

            _panelStack.OpenPanel(_mainCanvas, new PanelOptions()
            {
                PanelType = PanelType.Normal,
                CanClose = type => type is PanelType.Normal
            });

            _continueButton.interactable = PlayerPrefs.HasKey(_savePath);
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

            if (PlayerPrefs.GetInt(firstTimeKey, defaultValue: 0) == 1) // is not first time
            {
                SavePath path = _allSavesHandler.GetSaveFilePath(PlayerPrefs.GetInt(_savePath));
                StaticServices.Get<SaveService>().SavePath = path;

                Loader.TargetScene = File.Exists(path.RunPath) ? Scenes.MainScene : Scenes.Hub;
                _saverHandler.Load(path);
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
            SavePath saveFile = _allSavesHandler.GetSaveFilePath(saveNumber);
            StaticServices.Get<SaveService>().SavePath = saveFile;
            PlayerPrefs.SetInt(_savePath, saveNumber);

            _transition.SetTrigger("Start");
            StartCoroutine(Scenes.LoadAsync(Scenes.LoadingMenu));
            Loader.TargetScene = Scenes.Hub;
        }

        public void DeleteSave(int saveNumber)
        {
            int uiSaveNumber = saveNumber + 1;

            ModalWindow.Show(new ModalWindowData()
            {
                Header = $"Are you sure want to delete Save {uiSaveNumber.ToString()}",
                Content = "There is no way to undo this action",
                OkAction = () =>
                {
                    _allSavesHandler.DeleteSave(saveNumber);
                    ModalWindow.ShowMessage($"Save {uiSaveNumber.ToString()} deleted!");

                    foreach (var panel in _savesPanel.GetComponentsInChildren<SavePanel>())
                    {
                        panel.Start(); // refresh panel
                    }
                }
            });
        }

        public void OnQuitClick()
        {
            Application.Quit();
        }
    }
}
