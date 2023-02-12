using Core.Events;
using Core.Mappers;
using Core.Values;
using UnityEngine;

namespace Gameplay
{
    public class WishingWell : MonoBehaviour, IHubPointListener
    {
        [SerializeField] private ButtonEvent _interactionButton;

        public void OnEnter()
        {
            _interactionButton.Show(new SimpleButtonModel("Make a wish", () => { }, gameObject));
        }

        public void OnExit()
        {
            _interactionButton.Close(gameObject);
        }
    }
}
