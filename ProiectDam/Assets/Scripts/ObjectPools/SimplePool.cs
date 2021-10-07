using Object = UnityEngine.Object;
using UnityEngine;

namespace ObjectPooling
{
    public class SimplePool : IObjectPool
    {
        private readonly GameObject[] objects;
        private int current;

        public SimplePool(int size, GameObject prefab)
        {
            objects = new GameObject[size];
            current = size - 1;

            for (int i = 0; i < size; ++i)
            {
                objects[i] = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                objects[i].SetActive(false);
            }
        }

        public void Clear()
        {
            foreach (GameObject gameObject in objects)
            {
                GameObject.Destroy(gameObject);
            }
        }

        public GameObject Get(Vector3 where)
        {
            if (current == -1)
            {
                return null;
            }

            objects[current].SetActive(true);
            objects[current].transform.position = where;

            return objects[current--];
        }

        public void Return(GameObject gameObject)
        {
            if (current == objects.Length - 1)
            {
                return;
            }

            gameObject.SetActive(false);
            objects[++current] = gameObject;
        }
    }

    public class SimplePool<T> : IObjectPool<T> where T : MonoBehaviour
    {
        private readonly T[] objects;
        private int current;

        public SimplePool(int size, T behaviour)
        {
            objects = new T[size];
            current = size - 1;

            for (int i = 0; i < size; ++i)
            {
                objects[i] = Object.Instantiate(behaviour, Vector3.zero, Quaternion.identity);
                objects[i].gameObject.SetActive(false);
            }
        }

        public void Clear()
        {
            foreach (MonoBehaviour behaviour in objects)
            {
                GameObject.Destroy(behaviour.gameObject);
            }
        }

        public T Get(Vector3 where)
        {
            if (current == -1)
            {
                return null;
            }

            objects[current].gameObject.SetActive(true);
            objects[current].transform.position = where;

            return objects[current--];
        }

        public void Return(T gameObject)
        {
            if (current == objects.Length - 1)
            {
                return;
            }

            gameObject.gameObject.SetActive(false);
            objects[++current] = gameObject;
        }
    }
}