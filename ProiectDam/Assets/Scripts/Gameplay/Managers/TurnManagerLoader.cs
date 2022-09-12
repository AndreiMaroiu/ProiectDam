using Gameplay.DataSaving;
using UnityEngine;

namespace Gameplay.Managers
{
    public class TurnManagerLoader : MonoBehaviour
    {
        [SerializeField] private TurnManager _turnManager;
        [SerializeField] private RandomLevelSaverManager _levelSaver;

        private void Start()
        {
            if (_levelSaver.ShouldLoad)
            {
                _turnManager.LoadFromSave(_levelSaver.SaveData.TurnManagerData);
            }
        }
    }
}
