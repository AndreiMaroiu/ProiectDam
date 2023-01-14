using Core.Events;
using Core.Items;
using ModalWindows;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI.Merchant
{
    public class MerchantShopUI : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private ItemsEvent _itemsEvent;
        [SerializeField] private IntEvent _moneyEvent;
        [Header("UI Elements")]
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _panel;
        [SerializeField] private ItemDescriptionUI _itemPrefab;
        [SerializeField] private Text _moneyText;
        [SerializeField] private PanelStack _panelStack;

        private void Start()
        {
            _itemsEvent.OnItemShow += OnItemShow;
            _mainCanvas.SetActive(false);
        }

        private void OnDestroy()
        {
            _itemsEvent.OnItemShow -= OnItemShow;
        }

        private void OnItemShow(IEnumerable<ItemDescription> descriptions)
        {
            _panelStack.OpenDialog(_mainCanvas);
            ClearPanel();

            foreach (ItemDescription item in descriptions)
            {
                ItemDescriptionUI prefab = GameObject.Instantiate(_itemPrefab, _panel.transform);
                prefab.SetItem(item, () =>
                {
                    TryBuyItem(item);
                });
            }

            _moneyText.text = _moneyEvent.ToString();
        }

        private bool TryBuyItem(ItemDescription item)
        {
            if (item.Cost > _moneyEvent.Value)
            {
                ModalWindow.ShowDialog(new ModalWindowData()
                {
                    Header = "Not enough money!",
                });

                return false;
            }

            _moneyEvent.Value -= item.Cost;
            _moneyText.text = _moneyEvent.Value.ToString();
            _itemsEvent.BuyItem(item);

            ModalWindow.ShowDialog(new ModalWindowData()
            {
                Header = "Item bought!",
                Image = item.Image,
                Content = item.Name,
            });

            ClosePanel();

            return true;
        }

        private void ClearPanel()
        {
            for (int i = 0; i < _panel.transform.childCount; i++)
            {
                Transform child = _panel.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        public void ClosePanel()
        {
            _panelStack.ClosePanel();
        }
    }
}
