using Core;
using Core.DataSaving;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SavePanel : MonoBehaviour
    {
        [SerializeField] private Text _summary;
        [SerializeField] private AllSavesHandler _allSaves;
        [SerializeField] private int _saveNumber;

        private void Start()
        {
            SaveSummary save = _allSaves.Summaries[_saveNumber];

            if (save is not null)
            {
                _summary.text = $"Health: {save.Health.ToString()}\n" +
                    $"Energy: {save.Energy.ToString()}\n" +
                    $"Money: {save.Money.ToString()}\n" +
                    $"Rooms explored: {save.RoomsDiscovered.ToString()}";
            }
            else
            {
                _summary.text = "No save data found!";
            }
        }
    }
}
