using Core.Events;
using GameStatistics;
using ModalWindows;
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

        private bool _eventTriggered;

        private void Start()
        {
            _winEvent.OnEvent += OnWin;
            _loseEvent.OnEvent += OnPlayerDeath;
        }

        private void OnLose()
        {

        }

        private void OnPlayerDeath(object sender)
        {
            _eventTriggered = true;

            using StatsHandler<Statistics> stats = StatisticsManager.Instance.LoadStats();

            stats.Data.AddLoss();

            ModalWindow.Show(new ModalWindowData()
            {
                Header = "You Died!",
                Content = "Score: " + _scoreEvent.Value.ToString(),
                CloseText = "Main Menu",
                CloseAction = () => SceneManager.LoadScene(Scenes.MainMenu),
                OkText = "Play Again",
                OkAction = () => SceneManager.LoadScene(Scenes.MainScene)
            });
        }

        private void OnWin(object sender)
        {
            _eventTriggered = true;

            float timeScale = Time.timeScale;
            string footer = null;

            using StatsHandler<Statistics> stats = StatisticsManager.Instance.LoadStats();
            stats.Data.AddWin();
            
            if (_scoreEvent > stats.Data.Highscore)
            {
                stats.Data.Highscore = _scoreEvent;
                footer = "New high score!";
            }

            ModalWindow.ShowDialog(timeScale, new ModalWindowData()
            {
                Header = "You Won!",
                Content = "Score: " + _scoreEvent.Value.ToString(),
                Footer = footer,
                OkText = "Play again",
                OkAction = () => SceneManager.LoadScene(Scenes.MainScene),
                CloseText = "Main Menu",
                CloseAction = () => SceneManager.LoadScene(Scenes.MainMenu),
            });
        }

        private void OnDestroy()
        {
            _winEvent.OnEvent -= OnWin;
            _loseEvent.OnEvent -= OnPlayerDeath;

            // if player leave level without winning or losing save data
            if (!_eventTriggered)
            {
                using var stats = StatisticsManager.Instance.LoadStats();

                stats.Data.TotalRuns += 1;
            }
        }
    }
}
