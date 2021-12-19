using UnityEngine;
using Values;

namespace Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChestBehaviour : PressableObject
    {
        [SerializeField] private ChestHelper _helper;
        [SerializeField] private Sprite _openedChestImage;
        [SerializeField] private GameObject[] _pickUps;
        [SerializeField] private FloatValue _cellSize;

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

            Vector3 direction = (transform.parent.position - _helper.Controller.transform.position);
            direction.z = 0;
            direction.Normalize();

            Vector3 where = transform.parent.position + direction * _cellSize.Value;
            Instantiate(_pickUps[Random.Range(0, _pickUps.Length)], where, Quaternion.identity);

            _wasOpened = true;
        }

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
    }
}
