using Core.Events;
using Core.Items;
using ModalWindows;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private void Start()
        {
            _itemsEvent.OnItemShow += OnItemShow;
            ClosePanel();
        }

        private void OnDestroy()
        {
            _itemsEvent.OnItemShow -= OnItemShow;
        }

        private void OnItemShow(IEnumerable<ItemDescription> descriptions)
        {
            ShowPanel();
            ClearPanel();

            foreach (ItemDescription item in descriptions)
            {
                ItemDescriptionUI prefab = GameObject.Instantiate(_itemPrefab, _panel.transform);
                prefab.SetItem(item, () =>
                {
                    _itemsEvent.BuyItem(item);
                    ClosePanel();
                });
            }

            _moneyText.text = _moneyEvent.ToString();
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
            _mainCanvas.SetActive(false);
        }

        private void ShowPanel()
        {
            _mainCanvas.SetActive(true);
        }
    }
}
