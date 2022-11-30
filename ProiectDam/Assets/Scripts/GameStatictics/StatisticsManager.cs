using UnityEngine;
using BinaryReader = Utilities.BinaryReader;

namespace GameStatistics
{
    public class StatisticsManager
    {
        public static StatisticsManager Instance { get; } = new();

        private readonly string _statsPath;
        private readonly string _moneyPath;

        public StatisticsManager()
        {
            _statsPath = Application.persistentDataPath + "/Statistics.dat";
            _moneyPath = Application.persistentDataPath + "/Money.dat";
        }

        #region Public Methods

        public StatsHandler<Statistics> LoadStats() => LoadHandler<Statistics>(_statsPath);

        public StatsHandler<PlayerMoney> LoadMoney() => LoadHandler<PlayerMoney>(_moneyPath);

        public void Save(Statistics stats) => Save(stats, _statsPath);

        public void Save(PlayerMoney money) => Save(money, _moneyPath);

        #endregion

        #region Private Methods

        internal void Save<T>(T data, string path)
        {
            BinaryReader.Write(path, data);
        }

        public StatsHandler<T> LoadHandler<T>(string path) where T : new()
        {
            BinaryReader.TryRead(path, out T result);
            return new(result, this, path);
        }

        public T Read<T>(string path) where T : new()
        {
            BinaryReader.TryRead(path, out T result);
            return result;
        }

        #endregion
    }
}
