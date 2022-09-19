using UnityEngine;
using Core.Values;
using Gameplay.DataSaving;

namespace Gameplay
{
    public class ChestBehaviour : PressableObject, IDataSavingTile
    {
        [SerializeField] private ChestHelper _helper;
        [SerializeField] private Sprite _openedChestImage;
        [SerializeField] private GameObject[] _pickUps;
        [SerializeField] private FloatValue _cellSize;

        private SpriteRenderer _renderer;
        private bool _wasOpened = false;

        public string ObjectName { get; set; }

        public ObjectSaveData SaveData => new ChestSaveData()
        {
            ObjectName = ObjectName,
            WasOpened = _wasOpened
        };

        public void LoadFromSave(ObjectSaveData saveData)
        {
            if (saveData is ChestSaveData data)
            {
                _wasOpened = data.WasOpened;
            }
        }

        public override void OnClick()
        {
            if (!_helper.CanInteract || _wasOpened)
            {
                return;
            }

            _renderer.sprite = _openedChestImage;

            // todo: better spawn position
            Vector3 direction = (transform.position - _helper.Controller.transform.position);
            direction.z = 0;
            direction.Normalize();

            Vector3 where = transform.position + direction * _cellSize.Value;
            Instantiate(_pickUps[Random.Range(0, _pickUps.Length)], where, Quaternion.identity);

            _wasOpened = true;
        }

        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
}
