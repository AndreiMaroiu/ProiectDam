using Core;
using Gameplay.PickUps;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Merchant
{
    [Serializable]
    public struct ShopItem
    {
        public Item item;
        public BasePickUp interactable;
    }

    [CreateAssetMenu(fileName = "New Shop Items", menuName = "Scriptables/Shop Items")]
    public class ShopItems : ScriptableObject
    {
        [SerializeField]
        private ShopItem[] _items;

        public ShopItem[] Items => _items;

        public ShopItem this[int index] => _items[index];
    }
}
