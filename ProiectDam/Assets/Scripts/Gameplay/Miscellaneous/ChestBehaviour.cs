using UnityEngine;
using Values;

namespace Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChestBehaviour : PressebleObject
    {
        [SerializeField] private ChestHelper _helper;
        [SerializeField] private Sprite _openedChestImage;
        [SerializeField] private GameObject[] _pickUps;

        private SpriteRenderer _renderer;
        private bool _wasOpened = false;

        public override void OnClick()
        {
            if (!_helper.CanInteract || _wasOpened)
            {
                Debug.Log("Cannot interact with chest!");
                return;
            }

            Debug.Log("Chest opened!");

            _renderer.sprite = _openedChestImage;

            Vector3 where = transform.position - (_helper.Controller.transform.position - transform.position);
            Instantiate(_pickUps[Random.Range(0, _pickUps.Length)], where, Quaternion.identity);

            _wasOpened = true;
        }

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
    }
}
