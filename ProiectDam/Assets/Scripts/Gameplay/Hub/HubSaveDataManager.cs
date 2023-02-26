using Core;
using Core.DataSaving;
using Core.Events;
using Core.Services;
using Gameplay.DataSaving;
using Gameplay.PickUps;
using UnityEngine;
using Utilities;

namespace Gameplay.Hub
{
    public class HubSaveDataManager : MonoBehaviour
    {
        [SerializeField] private IntEvent _hubCoins;
        [SerializeField] private HubItemEvent _hubItemEvent;

        private SavePath _savePath;
        private HubSaveData _data;

        private void Start()
        {
            _savePath = StaticServices.Get<SaveService>().SavePath;

            if (DataReader.TryRead<HubSaveData>(_savePath.SaveDataPath, out var result))
            {
                _data = result;

                _hubCoins.Value = _data.Coins;
            }

            _hubItemEvent.OnEvent += OnItemEvent;
        }

        private void OnItemEvent(HubEventInfo info)
        {
            if (info.Type is HubItemType.Temporary)
            {
                _data.SingleTimePickUps.Add(PickUpFactory.Instance.GetSaveData(info.Item.GetPickUp())); // TODO: optimize
            }
        }

        private void OnDestroy()
        {
            DataReader.Write(_savePath.SaveDataPath, new HubSaveData()
            {
                Coins = _hubCoins.Value,
            });

            _hubItemEvent.OnEvent -= OnItemEvent;
        }
    }
}
