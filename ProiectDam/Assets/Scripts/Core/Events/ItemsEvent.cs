using Core.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Items Event", menuName = "Scriptables/Items/Items Event")]
    public class ItemsEvent : ScriptableObject
    {
        public event Action<IEnumerable<ItemDescription>> OnItemShow;
        public event Action<ItemDescription> OnItemBought;

        public void ShowItems(IEnumerable<ItemDescription> items)
        {
            OnItemShow?.Invoke(items);
        }

        public void BuyItem(ItemDescription item)
        {
            OnItemBought?.Invoke(item);
        }
    }
}
