using Core.Events;
using GameStatistics;
using ModalWindows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Gameplay
{
    public class WinManager : MonoBehaviour
    {
        [SerializeField] private GameEvent _winEvent;
        [SerializeField] private IntEvent _scoreEvent;

        private void Start()
        {
            _winEvent.OnEvent += OnWin;
        }

        private void OnWin(object sender)
        {
            float timeScale = Time.timeScale;
            Time.timeScale = 0;

            string footer = null;

            Statistics stats = StatisticsManager.Instance.LoadStats();
            stats.AddWin();
            
            if (_scoreEvent > stats.Highscore)
            {
                stats.Highscore = _scoreEvent;
                footer = "New high score!";
            }

            StatisticsManager.Instance.Save(stats);

            ModalWindow.Show(new ModalWindowData()
            {
                Header = "You Won!",
                Content = "Score: " + _scoreEvent.Value.ToString(),
                Footer = footer,
                OkText = "Play again",
                OkAction = () =>
                {
                    Time.timeScale = timeScale;
                    SceneManager.LoadScene(Scenes.MainScene);
                },
                CloseText = "Main Menu",
                CloseAction = () =>
                {
                    Time.timeScale = timeScale;
                    SceneManager.LoadScene(Scenes.MainMenu);
                }
            });
        }

        private void OnDestroy()
        {
            _winEvent.OnEvent -= OnWin;
        }
    }
}
