using UnityEngine;
using Core.Values;
using Gameplay.DataSaving;
using Core.Events;
using Core.Mappers;

namespace Gameplay
{
    public class ChestBehaviour : PressableObject, IDataSavingTile
    {
        [SerializeField] private InteractionHelper _helper;
        [SerializeField] private Sprite _openedChestImage;
        [SerializeField] private GameObject[] _pickUps;
        [SerializeField] private FloatValue _cellSize;
        [SerializeField] private ButtonEvent _buttonEvent;

        private SpriteRenderer _renderer;

        public string ObjectName { get; set; }

        public ObjectSaveData SaveData => new ChestSaveData()
        {
            ObjectName = ObjectName,
            WasOpened = !_helper.CanClick
        };

        public void LoadFromSave(ObjectSaveData saveData)
        {
            if (saveData is ChestSaveData data)
            {
                _helper.CanClick = !data.WasOpened;
            }
        }

        public override void OnClick()
        {
            if (!_helper.IsInteracting || !_helper.CanClick)
            {
                return;
            }

            _renderer.sprite = _openedChestImage;

            // todo: better spawn position
            Vector3 direction = (transform.position - _helper.Controller.transform.position);
            direction.z = 0;

            Vector3 where = transform.position + direction;
            Instantiate(_pickUps[Random.Range(0, _pickUps.Length)], where, Quaternion.identity);

            _helper.CanClick = false;
        }

        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _helper.Set(new SimpleButtonModel("Open Chest", OnClick));
            _helper.CanClick = true;
        }
    }
}
