using Gameplay.PickUps;
using UnityEngine;

namespace Gameplay.Merchant
{
    [CreateAssetMenu(fileName = "New Shop Items", menuName = "Scriptables/Shop Items")]
    public class ShopItems : ScriptableObject
    {
        [SerializeField]
        private Item[] _items;

        public Item[] Items => _items;

        public Item this[int index] => _items[index];
    }
}
