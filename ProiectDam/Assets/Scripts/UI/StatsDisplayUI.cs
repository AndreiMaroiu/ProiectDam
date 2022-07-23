using GameStatistics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsDisplayUI : MonoBehaviour
    {
        [SerializeField] private Text _runsText;

        private void Start()
        {
            Statistics stats = StatisticsManager.Instance.LoadStats();

            _runsText.text = stats.TotalRuns.ToString();
        }
    }
}
