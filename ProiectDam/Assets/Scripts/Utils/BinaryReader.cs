using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities
{
    public static class BinaryReader
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
    }
}
