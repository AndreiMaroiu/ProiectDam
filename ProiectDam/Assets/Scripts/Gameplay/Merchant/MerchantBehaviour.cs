using Core;
using Core.Events;
using Core.Items;
using Gameplay.PickUps;
using Gameplay.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Merchant
{
    public class MerchantBehaviour : PressableObject, IInteractable
    {
        [SerializeField] private Transform _dialogPosition;
        [SerializeField] private GameObject _dialogCanvas;
        [SerializeField] private Text _dialogText;
        [SerializeField] private VerticalLayoutGroup _layout;
        [SerializeField] private ShopItems _items;
        [SerializeField] private ItemsEvent _itemsEvent;

        private Animator _animator;
        private PlayerController _player;
        private bool _canBuy = true;

        #region IInteractable

        public void OnInteract(PlayerController controller)
        {
            _player = controller;

            string dialogLine = ChooseRandomDialog();
            StartCoroutine(ShowDialog(dialogLine, 2.0f));

            _animator.SetTrigger("Open");
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            StartCoroutine(ShowDialog("Bye!", 1.0f));

            _animator.SetTrigger("Hide");

            _player = null;
        }

        #endregion

        #region Unity Events

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _dialogCanvas.SetActive(false);

            _itemsEvent.OnItemBought += OnBuy;
        }

        private void OnDestroy()
        {
            _itemsEvent.OnItemBought -= OnBuy;
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

            _itemsEvent.ShowItems(_items.Items);
        }

        #endregion

        private string ChooseRandomDialog()
        {
            int value = Random.Range(0, 10);

            if (value < 5)
            {
                return "Hi!";
            }

            return "Hello!";
        }

        IEnumerator ShowDialog(string dialogText, float openDuration = 1.0f)
        {
            //StopAllCoroutines();

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
            }
        }
    }
}
