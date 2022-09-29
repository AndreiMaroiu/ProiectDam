using Gameplay.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using System.Collections;
using UnityEngine;

namespace Gameplay.Managers
{
    public class PlayerLoader : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private RandomLevelSaverManager _saverManager;

        private void Start()
        {
            if (_saverManager.ShouldLoad)
            {
                _player.LoadFromSave(_saverManager.SaveData.PlayerData);
            }
        }
    }
}
