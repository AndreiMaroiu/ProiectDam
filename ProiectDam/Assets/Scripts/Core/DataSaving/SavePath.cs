using System.IO;

namespace Core.DataSaving
{
    public readonly struct SavePath
    {
        private readonly string _path;

        public SavePath(string path)
        {
            _path = path;
        }

        public string SummaryPath => _path + ".summary.dat";
        public string SaveDataPath => _path + ".dat";

        public bool Exists() => File.Exists(SummaryPath) && File.Exists(SaveDataPath);

        public bool IsNullOrEmpty() => string.IsNullOrEmpty(_path);

        public static implicit operator SavePath(string path)
            => new(path);
    }
}
