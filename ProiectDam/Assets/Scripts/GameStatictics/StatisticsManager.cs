using System;
using System.Collections.Generic;
using UnityEngine;
using DataReader = Utilities.DataReader;

namespace GameStatistics
{
    public class StatisticsManager
    {
        public static StatisticsManager Instance { get; } = new();

        private readonly string _statsPath;
        private readonly string _moneyPath;

        private readonly Dictionary<Type, object> _shared = new();

        public StatisticsManager()
        {
            _statsPath = Application.persistentDataPath + "/Statistics.dat";
            _moneyPath = Application.persistentDataPath + "/Money.dat";
        }

        #region Public Methods

        public PersistentHandler<Statistics> LoadStats() => LoadHandler<Statistics>(_statsPath);

        public PersistentHandler<PlayerMoney> LoadMoney() => LoadHandler<PlayerMoney>(_moneyPath);

        public void Save(Statistics stats) => Save(stats, _statsPath);

        public void Save(PlayerMoney money) => Save(money, _moneyPath);

        public PersistentHandler<T> LoadShared<T>() where T : new()
        {
            if (_shared.ContainsKey(typeof(T)))
            {
                return new PersistentHandler<T>((T)_shared[typeof(T)], GetPersistentPath(typeof(T)));
            }
            else
            {
                var handler = LoadHandler<T>(GetPersistentPath(typeof(T)));
                _shared[typeof(T)] = handler.Data;
                return handler;
            }
        }

        private static string GetPersistentPath(Type type)
        {
            return $"{Application.persistentDataPath}/{type.Name}.dat";
        }

        #endregion

        #region Private Methods

        internal void Save<T>(T data, string path)
        {
            DataReader.Write(path, data);
        }

        internal PersistentHandler<T> LoadHandler<T>(string path) where T : new()
        {
            DataReader.TryRead(path, out T result);
            return new(result, path);
        }

        #endregion
    }
}
