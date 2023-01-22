using Core;
using Core.Events;
using Core.Events.Binding;
using ModalWindows;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class AdvanceTutorialManager : BaseBindableBehaviour
    {
        [Header("Events")]
        [SerializeField] private RoomEvent _onRoomEvent;
        [Header("Settings")]
        [SerializeField] private float _roomMessageWaitTime;

        private bool _merchantVisited = false;
        private bool _bossVisited = false;
        private bool _chestVisited = false;

        private void Start()
        {
            Bind(_onRoomEvent, OnRoomChanged);

            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "Welcome to the advanced tutorial!"
            });
        }

        private void OnRoomChanged(Room room)
        {
            switch (room.Type)
            {
                case RoomType.Merchant when _merchantVisited is false:
                    _merchantVisited = true;
                    OnMerchantEnter();
                    break;
                case RoomType.Boss when _bossVisited is false:
                    _bossVisited = true;
                    OnBossEnter();
                    break;
                case RoomType.Chest when _chestVisited is false:
                    _chestVisited = true;
                    OnChestEnter();
                    break;
                default:
                    break;
            }
        }

        private void OnBossEnter()
        {
            StartCoroutine(ShowModal(new ModalWindowData()
            {
                Header = "You entered the boss room",
                Content = "Kill all three bosses and collect a key, in order to escape",
                Footer = "Make sure you have enough resource when entering a boss room",
            }));
        }

        private void OnMerchantEnter()
        {
            StartCoroutine(ShowModal(new ModalWindowData()
            {
                Header = "You entered the merchant room",
                Content = "You can buy pick-ups from the merchant to help you in your journey!",
                Footer = "Move near the merchant to interact with him",
            }));
        }

        private void OnChestEnter()
        {
            StartCoroutine(ShowModal(new ModalWindowData()
            {
                Header = "You found a chest!",
                Content = "Open the chest to find some useful pickups"
            }));
        }

        private IEnumerator ShowModal(ModalWindowData data)
        {
            yield return new WaitForSeconds(_roomMessageWaitTime);

            ModalWindow.ShowDialog(data);
        }
    }
}
