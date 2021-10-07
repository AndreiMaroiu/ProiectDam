using UnityEngine;

namespace ObjectPooling
{
    public interface IObjectPool
    {
        GameObject Get(Vector3 where);
        void Return(GameObject gameObject);
        void Clear();
    }

    public interface IObjectPool<T> where T : MonoBehaviour
    {
        T Get(Vector3 where);
        void Return(T behaviour);
        void Clear();
    }
}
