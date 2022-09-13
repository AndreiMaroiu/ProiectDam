using Core.DataSaving;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SavePanel : MonoBehaviour
    {
        [SerializeField] private Text _summary;
        [SerializeField] private LevelSaverManager _saverManager;
        [SerializeField] private int _saveNumber;

        private void Start()
        {
            //if (_saverManager.IsSaveFilePopulated(_saveNumber))
            //{
            //    _summary.text = _saverManager.GetSaveSummary(_saveNumber);
            //}
            //else
            //{
            //    _summary.text = "No save data found!";
            //}

            _summary.text = "No save data found!";
        }
    }
}
