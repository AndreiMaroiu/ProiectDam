using Core.Items;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Merchant
{
    public class ItemDescriptionUI : MonoBehaviour
    {
        [SerializeField] private Text _description;
        [SerializeField] private Image _image;
        [SerializeField] private Text _priceText;

        private Action _buyCallback;

        public void SetItem(ItemDescription description, Action onBuy)
        {
            _buyCallback = onBuy;
            _description.text = description.Description;
            _priceText.text = description.Cost.ToString();
            _image.sprite = description.Image;
        }

        public void OnBuy()
        {
            _buyCallback?.Invoke();
        }
    }
}
