using Core.Events;
using Core.Items;
using Gameplay.PickUps;
using Gameplay.Player;
using GameStatistics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Merchant
{
    public class MerchantBehaviour : PressableObject, IInteractableEnter, IInteractableLeave
    {
        [SerializeField] private Transform _dialogPosition;
        [SerializeField] private GameObject _dialogCanvas;
        [SerializeField] private Text _dialogText;
        [SerializeField] private VerticalLayoutGroup _layout;
        [SerializeField] private ShopItems _items;
        [SerializeField] private ItemsEvent _itemsEvent;
        [SerializeField] private MerchantDialogs _dialogs;

        private Animator _animator;
        private PlayerController _player;
        private bool _canBuy = true;
        private Item[] _itemsToShow;
        private bool _isOpen = false;
        private StatsHandler<MerchantData> _saveData;

        #region IInteractable

        public void OnInteract(PlayerController controller)
        {
            if (_isOpen)
            {
                return;
            }

            _isOpen = true;
            _player = controller;

            string dialogLine = ChooseRandomDialog();
            StartCoroutine(ShowDialog(dialogLine, 2.0f));

            _animator.SetTrigger("Open");
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            if (!_isOpen)
            {
                return;
            }

            StartCoroutine(ShowDialog("Bye!", 1.0f));

            _animator.SetTrigger("Hide");
            _player = null;
            _isOpen = false;
        }

        #endregion

        #region Unity Events

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _dialogCanvas.SetActive(false);

            _itemsEvent.OnItemBought += OnBuy;
            _itemsToShow = GenerateRandomItemList();
            _saveData = StatisticsManager.Instance.LoadHandler<MerchantData>(Application.persistentDataPath + "/MerchantData.dat");
        }

        private void OnDestroy()
        {
            _itemsEvent.OnItemBought -= OnBuy;

            _saveData?.Dispose();
        }

        #endregion

        #region Pressable Object

        public override void OnClick()
        {
            if (_player is null)
            {
                return;
            }

            if (!_canBuy)
            {
                StartCoroutine(ShowDialog("Sorry, no more items for now!", 2.0f));
                return;
            }

            _itemsEvent.ShowItems(_itemsToShow);
        }

        #endregion

        private string ChooseRandomDialog()
        {
            return _dialogs.GetRandomDialog(_saveData.Data.FriendshipLevel);
        }

        private IEnumerator ShowDialog(string dialogText, float openDuration = 1.0f)
        {
            _dialogCanvas.SetActive(true);
            _dialogCanvas.transform.position = _dialogPosition.position;
            _dialogText.text = dialogText;
            _layout.enabled = false;

            yield return null;
            _layout.enabled = true;

            yield return new WaitForSeconds(openDuration);

            _dialogCanvas.SetActive(false);
        }

        private void OnBuy(ItemDescription description)
        {
            if (description is Item item)
            {
                item.GetPickUp().OnInteract(_player);
                _canBuy = false;

                _saveData.Data.Increment();
            }
        }

        private Item[] GenerateRandomItemList()
        {
            const int itemsCount = 3;
            Item[] itemsToShow = new Item[itemsCount];

            RandomPicker<Item> picker = new RandomPicker<Item>(_items.Items);

            for (int i = 0; i < itemsCount; i++)
            {
                itemsToShow[i] = picker.Take();
            }

            return itemsToShow;
        }
    }
}
