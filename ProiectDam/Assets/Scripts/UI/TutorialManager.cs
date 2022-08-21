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
        [Header("Settings")]
        [SerializeField] private float _roomMessageWaitTime;

        private bool _normalEntered;
        private bool _endEntered;


        private void Awake()
        {
            _onGlobalKillEvent.OnEvent += OnKillMessage;
            _OnRoomEvent.OnValueChanged += OnRoomEnter;

            ShowWelcomeMessage();
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

            ModalWindow.ShowDialog(Time.timeScale, new ModalWindowData()
            {
                Header = "You entered a new room",
                Content = "Kill enemies with your pistol or knife",
            });
        }

        private IEnumerator ShowEndRoomMessage()
        {
            yield return new WaitForSeconds(_roomMessageWaitTime);

            ModalWindow.ShowDialog(Time.timeScale, new ModalWindowData()
            {
                Header = "You entered the finish room!",
                Content = "Move towards the portal in the middle in order to win the game!",
                Footer = "You can always check the \"How to play\" panel for more info or to replay the tutorial"
            });
        }

        private void OnKillMessage(object sender)
        {
            _onGlobalKillEvent.OnEvent -= OnKillMessage;

            ModalWindow.ShowDialog(Time.timeScale, new ModalWindowData()
            {
                Header = "Enemy killed!",
                Content = "Congratiolations, you killed your first enemy!"
            });
        }

        private void ShowWelcomeMessage()
        {
            ModalWindow.ShowDialog(Time.timeScale, new ModalWindowData()
            {
                Header = "Welcome to Nera!",
                Content = "Swipe the screen to move up, down, left or right\n\n" +
                "Try to move towards the door to reach the next room",
            });
        }
    }
}
