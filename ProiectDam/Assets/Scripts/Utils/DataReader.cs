using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities
{
    public static class DataReader
    {
        public static void Write<T>(string path, T data)
        {
            using FileStream file = File.Create(path);
            BinaryFormatter bf = new();

            bf.Serialize(file, data);
        }

        public static bool TryRead<T>(string path, out T result) where T : new()
        {
            if (!File.Exists(path))
            {
                result = new T();
                return false;
            }

            try
            {
                using FileStream file = File.OpenRead(path);
                BinaryFormatter bf = new();
                result = (T)bf.Deserialize(file);
                return true;
            }
            catch (System.Exception)
            {
                result = new T();
                return false;
            }
        }

        public static bool ReadAndSave<T>(string path, Action<T> process) where T : class, new()
        {
            if (TryRead<T>(path, out var data))
            {
                process(data);
                Write(path, data);
                return true;
            }

            return false;
        }
    }
}