using Core;
using Core.DataSaving;
using Core.Events;
using Core.Services;
using Gameplay.DataSaving;
using UnityEngine;
using Utilities;

namespace Gameplay.Hub
{
    public class HubSaveDataManager : MonoBehaviour
    {
        [SerializeField] private IntEvent _hubCoins;

        private SavePath _savePath;
        private HubSaveData _data;

        private void Start()
        {
            _savePath = StaticServices.Get<SaveService>().SavePath;

            if (BinaryReader.TryRead<HubSaveData>(_savePath.SaveDataPath, out var result))
            {
                _data = result;

                _hubCoins.Value = _data.Coins;
            }
        }

        private void OnDestroy()
        {
            BinaryReader.Write(_savePath.SaveDataPath, new HubSaveData()
            {
                Coins = _hubCoins.Value,
            });
        }
    }
}
