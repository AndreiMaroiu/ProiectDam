using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
            using FileStream file = File.Create(path);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(file, data);
        }

        private StatsHandler<T> Load<T>(string path) where T : new()
        {
            if (!File.Exists(path))
            {
                return new StatsHandler<T>(new T(), this, path);
            }

            using FileStream file = File.OpenRead(_statsPath);
            BinaryFormatter bf = new BinaryFormatter();
            T data = (T)bf.Deserialize(file);

            return new StatsHandler<T>(data, this, path);
        }

        #endregion
    }
}
