using GameStatistics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsDisplayUI : MonoBehaviour
    {
        [SerializeField] private Text _runsText;
        [SerializeField] private Text _winsText;
        [SerializeField] private Text _lossesText;
        [SerializeField] private Text _highText;

        private void Start()
        {
            Statistics stats = StatisticsManager.Instance.LoadStats();

            _runsText.text = stats.TotalRuns.ToString();
            _winsText.text = stats.Wins.ToString();
            _lossesText.text = stats.Losses.ToString();
            _highText.text = stats.Highscore.ToString();
        }
    }
}
