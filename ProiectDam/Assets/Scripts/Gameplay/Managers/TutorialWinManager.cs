using Core.Events;
using ModalWindows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Gameplay.Managers
{
    public class TutorialWinManager : MonoBehaviour
    {
        [SerializeField] private GameEvent _winEvent;
        [SerializeField] private GameEvent _loseEvent;

        private void Start()
        {
            _winEvent.OnEvent += OnWin;
            _loseEvent.OnEvent += OnLose;
        }

        private void OnDestroy()
        {
            _winEvent.OnEvent -= OnWin;
            _loseEvent.OnEvent -= OnLose;
        }

        private void OnWin(object sender)
        {
            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "Tutorial finished!",
                Content = "You can always check the \"How to play\" panel for more info or to replay the tutorial",
                CloseText = "Main Menu",
                CloseAction = () => SceneManager.LoadScene(Scenes.MainMenu),
            });
        }

        private void OnLose(object sender)
        {
            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "You died!",
                Content = "Let's try again, but be more careful",
                CloseText = "Replay tutorial",
                CloseAction = () => SceneManager.LoadScene(Scenes.Tutorial),
            });
        }
    }
}
