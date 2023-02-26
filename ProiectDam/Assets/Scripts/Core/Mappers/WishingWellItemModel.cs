using Core.Items;
using System;
using UnityEngine;

namespace Core.Mappers
{
    public sealed class WishingWellItemModel : IButtonModel
    {
        public GameObject Owner { get; set; }

        public Func<ItemDescription> ItemGenerator { get; set; }

        public int RequiredCoins { get; set; }

        public int CurrentCoins { get; set; }
    }
}
