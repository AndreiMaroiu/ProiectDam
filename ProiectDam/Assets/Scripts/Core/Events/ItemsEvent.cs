using Core.Events.Binding;
using Core.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Items Event", menuName = "Scriptables/Items/Items Event")]
    public sealed class ItemsEvent : ScriptableObject
    {
        private readonly BindableEvent<IEnumerable<ItemDescription>> _onItemsShow = new();
        private readonly BindableEvent<ItemDescription> _onItemBought = new();

        public event Action<IEnumerable<ItemDescription>> OnItemShow
        {
            add => _onItemsShow.OnValueChanged += value;
            remove => _onItemsShow.OnValueChanged -= value;
        }
        public event Action<ItemDescription> OnItemBought
        {
            add => _onItemBought.OnValueChanged += value;
            remove => _onItemBought.OnValueChanged -= value;
        }

        public void ShowItems(IEnumerable<ItemDescription> items) => _onItemsShow.Invoke(items);

        public void BuyItem(ItemDescription item) => _onItemBought.Invoke(item);

        public IBindable<IEnumerable<ItemDescription>> ItemsShowBindable => _onItemsShow;
        public IBindable<ItemDescription> ItemBoughtBindable => _onItemBought;
    }
}
