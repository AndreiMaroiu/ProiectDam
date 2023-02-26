using Core.Events;
using Core.Mappers;
using Core.Values;
using Gameplay.Hub;
using Gameplay.PickUps;
using UnityEngine;

namespace Gameplay
{
    public class WishingWell : MonoBehaviour, IHubPointListener
    {
        [SerializeField] private ButtonEvent _interactionButton;
        [SerializeField] private IntEvent _hubCoins;
        [SerializeField] private int _requiredCoins;
        [SerializeField] private HubItemEvent _hubItemEvent;
        [SerializeField] private Item[] _items;

        public void OnEnter()
        {
            _interactionButton.Show(new WishingWellItemModel()
            {
                Owner = gameObject,
                CurrentCoins = _hubCoins,
                RequiredCoins = _requiredCoins,
                ItemGenerator = () =>
                {
                    if (_hubCoins < _requiredCoins)
                    {
                        throw new System.Exception("Not enough coins");
                    }

                    _hubCoins.Value -= _requiredCoins;
                    Item item = new RandomPicker<Item>(_items).Take();

                    _hubItemEvent.Invoke(new HubEventInfo()
                    {
                        Item = item,
                        Type = HubItemType.Temporary,
                    });
                    
                    return item;
                }
            });
        }

        public void OnExit()
        {
            _interactionButton.Close(gameObject);
        }
    }
}
