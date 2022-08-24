using Core;
using Core.Events;
using ModalWindows;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameEvent _onGlobalKillEvent;
        [SerializeField] private RoomEvent _OnRoomEvent;
        [SerializeField] private GameEvent _onPlayerMoveEnded;
        [Header("Settings")]
        [SerializeField] private float _roomMessageWaitTime;
        [Header("Icons")]
        [SerializeField] private Sprite _loopIcon;

        private bool _normalEntered;
        private bool _endEntered;


        private void Start()
        {
            _onGlobalKillEvent.OnEvent += OnKillMessage;
            _OnRoomEvent.OnValueChanged += OnRoomEnter;
            _onPlayerMoveEnded.OnEvent += ShowMoveMessage;

            ShowWelcomeMessage();
        }

        private void OnDestroy()
        {
            _onGlobalKillEvent.OnEvent -= OnKillMessage;
            _OnRoomEvent.OnValueChanged -= OnRoomEnter;
            _onPlayerMoveEnded.OnEvent -= ShowMoveMessage;
        }

        private void OnRoomEnter()
        {
            switch (_OnRoomEvent.Value.Type)
            {
                case RoomType.Normal when _normalEntered is false:
                    StartCoroutine(ShowNormalRoomMessage());
                    _normalEntered = true;
                    break;
                case RoomType.End when _endEntered is false:
                    StartCoroutine(ShowEndRoomMessage());
                    _endEntered = true;
                    break;
                default:
                    break;
            }
        }

        private IEnumerator ShowNormalRoomMessage()
        {
            yield return new WaitForSeconds(_roomMessageWaitTime);

#pragma warning disable HAA0101 // Array allocation for params parameter
            ModalWindow.ShowPages(new ModalWindowPageData()
            {
                Header = "You entered a new room",
                Content = "Kill enemies with your pistol or knife",
            },
            new ModalWindowPageData()
            {
                Header = "You can change dimensions",
                Content = "You the preview button to switch dimensions if you feel stuck!",
                Image = _loopIcon,
            });
#pragma warning restore HAA0101 // Array allocation for params parameter
        }

        private IEnumerator ShowEndRoomMessage()
        {
            yield return new WaitForSeconds(_roomMessageWaitTime);

            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "You entered the finish room!",
                Content = "Move towards the portal in the middle in order to win the game!",
            });
        }

        private void OnKillMessage(object sender)
        {
            _onGlobalKillEvent.OnEvent -= OnKillMessage;

            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "Enemy killed!",
                Content = "Congratiolations, you killed your first enemy!"
            });
        }

        private void ShowWelcomeMessage()
        {
            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "Welcome to Nera!",
                Content = "Swipe the screen to move up, down, left or right\n\n" +
                "Try to move towards the door to reach the next room",
            });
        }

        private void ShowMoveMessage(object sender)
        {
            _onPlayerMoveEnded.OnEvent -= ShowMoveMessage;

            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "You did your first move!",
                Content = "Beware when you move you use your energy points.\n" +
                "When your energy reaches 0 you lose!",
            });
        }
    }
}
