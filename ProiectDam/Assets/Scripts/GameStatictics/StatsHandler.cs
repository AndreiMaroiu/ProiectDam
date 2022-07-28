using System;

namespace GameStatistics
{
    public class StatsHandler<T> : IDisposable
    {
        private readonly StatisticsManager _manager;
        private readonly string _path;
        private readonly T _data;

        private bool _disposed;

        internal StatsHandler(T data, StatisticsManager manager, string savePath)
        {
            _data = data;
            _manager = manager;
            _path = savePath;
        }

        public T Data => _data;

        public void Dispose() => _manager.Save(_data, _path);

        public static implicit operator T(StatsHandler<T> handler) => handler.Data;

        ~StatsHandler()
        {
            if (_disposed)
            {
                return;
            }

            Dispose();
        }
    }
}
