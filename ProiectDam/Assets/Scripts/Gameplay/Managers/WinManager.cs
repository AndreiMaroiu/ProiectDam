using Core.DataSaving;
using Core.Events;
using GameStatistics;
using ModalWindows;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Gameplay.Managers
{
    public class WinManager : MonoBehaviour
    {
        [SerializeField] private GameEvent _winEvent;
        [SerializeField] private IntEvent _scoreEvent;
        [SerializeField] private GameEvent _loseEvent;
        [SerializeField] private LevelSaverHandler _levelSaverHandler;

        private void Start()
        {
            _winEvent.OnEvent += OnWin;
            _loseEvent.OnEvent += OnPlayerDeath;
        }

        private void OnPlayerDeath(object sender)
        {
            using PersistentHandler<Statistics> stats = StatisticsManager.Instance.LoadStats();

            stats.Data.AddLoss();

            ModalWindow.Show(new ModalWindowData()
            {
                Header = "You Died!",
                Content = "Score: " + _scoreEvent.Value.ToString(),
                CloseText = "Go to hub",
                CloseAction = () => SceneManager.LoadScene(Scenes.Hub),
                OkText = "Play Again",
                OkAction = () => SceneManager.LoadScene(Scenes.MainScene)
            });

            OnEvent();
        }

        private void OnWin(object sender)
        {
            string footer = null;

            using PersistentHandler<Statistics> stats = StatisticsManager.Instance.LoadStats();
            stats.Data.AddWin();
            
            if (_scoreEvent > stats.Data.Highscore)
            {
                stats.Data.Highscore = _scoreEvent;
                footer = "New high score!";
            }

            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "You Won!",
                Content = "Score: " + _scoreEvent.Value.ToString(),
                Footer = footer,
                OkText = "Play again",
                OkAction = () => SceneManager.LoadScene(Scenes.MainScene),
                CloseText = "Go to Hub",
                CloseAction = () => SceneManager.LoadScene(Scenes.Hub),
            });

            OnEvent();
        }

        private void OnEvent()
        {
            string path = _levelSaverHandler.SaveFile.RunPath;

            File.Delete(path);
        }

        private void OnDestroy()
        {
            _winEvent.OnEvent -= OnWin;
            _loseEvent.OnEvent -= OnPlayerDeath;
        }
    }
}
