using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChestBehaviour : PressebleObject
    {
        [SerializeField] private ChestHelper _helper;
        [SerializeField] private Sprite _openedChestImage;

        private SpriteRenderer _renderer;

        public override void OnClick()
        {
            if (!_helper.CanInteract)
            {
                return;
            }

            _renderer.sprite = _openedChestImage;
        }

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
    }
}
