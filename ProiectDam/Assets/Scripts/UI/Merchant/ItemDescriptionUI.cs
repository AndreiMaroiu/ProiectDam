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

        private Action _buyCallback;

        public void SetItem(ItemDescription description, Action onBuy)
        {
            _buyCallback = onBuy;
            _description.text = description.Description;
            _image.sprite = description.Image;
        }

        public void OnBuy()
        {
            _buyCallback?.Invoke();
        }
    }
}
