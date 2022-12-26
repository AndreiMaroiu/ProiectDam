using System;

namespace GameStatistics
{
    public class PersistentHandler<T> : IDisposable
    {
        private readonly string _path;
        private readonly T _data;

        private bool _disposed;

        internal PersistentHandler(T data, string savePath)
        {
            _data = data;
            _path = savePath;
        }

        public T Data => _data;

        public void Dispose()
        {
            StatisticsManager.Instance.Save(_data, _path);

            _disposed = true;
        }

        public static implicit operator T(PersistentHandler<T> handler) => handler.Data;

        ~PersistentHandler()
        {
            if (_disposed)
            {
                return;
            }

            Dispose();
        }
    }
}
