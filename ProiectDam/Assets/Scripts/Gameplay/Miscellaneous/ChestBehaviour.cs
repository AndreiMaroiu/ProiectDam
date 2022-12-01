using UnityEngine;
using Core.Values;
using Gameplay.DataSaving;
using Core.Events;

namespace Gameplay
{
    public class ChestBehaviour : PressableObject, IDataSavingTile
    {
        [SerializeField] private ChestHelper _helper;
        [SerializeField] private Sprite _openedChestImage;
        [SerializeField] private GameObject[] _pickUps;
        [SerializeField] private FloatValue _cellSize;
        [SerializeField] private ButtonEvent _buttonEvent;

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
                _helper.CanClick = _wasOpened;
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

            Vector3 where = transform.position + direction;
            Instantiate(_pickUps[Random.Range(0, _pickUps.Length)], where, Quaternion.identity);

            _wasOpened = true;
            _helper.CanClick = false;
        }

        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _helper.OnClick = OnClick;
            _helper.CanClick = true;
        }
    }
}
