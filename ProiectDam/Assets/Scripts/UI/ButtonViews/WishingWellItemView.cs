using Core.Mappers;
using ModalWindows;
using System;
using UnityEngine;

namespace UI.ButtonViews
{
    internal sealed class WishingWellItemView : IButtonView
    {
        private readonly Sprite _coinSprite;

        public WishingWellItemView(Sprite coinSprite)
        {
            _coinSprite = coinSprite;
        }

        public void Configure(IButtonModel model, ButtonData button)
        {
            if (model is not WishingWellItemModel itemModel)
            {
                return;
            }

            button.Button.interactable = itemModel.CurrentCoins >= itemModel.RequiredCoins;
            button.Text.text = $"Make a wish for {itemModel.RequiredCoins}";
            button.Icon.gameObject.SetActive(true);
            button.Icon.sprite = _coinSprite;

            button.Button.onClick.AddListener(() =>
            {
                try
                {
                    var description = itemModel.ItemGenerator();

                    ModalWindow.ShowDialog(new ModalWindowData()
                    {
                        Header = $"You got: {description.Name}",
                        Content = description.Description,
                        Image = description.Image,
                    });
                }
                catch (Exception e)
                {
                    ModalWindow.ShowDialog(new ModalWindowData()
                    {
                        Header = "Something went wrong",
                        Content = e.Message,
                    });
                }
            });
        }
    }
}
