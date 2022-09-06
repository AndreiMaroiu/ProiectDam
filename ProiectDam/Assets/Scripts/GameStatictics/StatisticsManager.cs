using UnityEngine;
using BinaryReader = Utilities.BinaryReader;

namespace GameStatistics
{
    public class StatisticsManager
    {
        public static StatisticsManager Instance { get; } = new StatisticsManager();

        private readonly string _statsPath;
        private readonly string _moneyPath;

        public StatisticsManager()
        {
            _statsPath = Application.persistentDataPath + "/Statistics.dat";
            _moneyPath = Application.persistentDataPath + "/Money.dat";
        }

        #region Public Methods

        public StatsHandler<Statistics> LoadStats() => Load<Statistics>(_statsPath);

        public StatsHandler<PlayerMoney> LoadMoney() => Load<PlayerMoney>(_moneyPath);

        public void Save(Statistics stats) => Save(stats, _statsPath);

        public void Save(PlayerMoney money) => Save(money, _moneyPath);

        #endregion

        #region Private Methods

        internal void Save<T>(T data, string path)
        {
            BinaryReader.Write(path, data);
        }

        private StatsHandler<T> Load<T>(string path) where T : new()
        {
            BinaryReader.TryRead(path, out T result);
            return new StatsHandler<T>(result, this, path);
        }

        #endregion
    }
}
