using UnityEngine;

namespace ObjectPooling
{
    public class RandomPool : IObjectPool
    {
        private readonly GameObject[] objects;
        private int size;

        public RandomPool(GameObject[] gameObjects, int copies = 1)
        {
            objects = new GameObject[gameObjects.Length * copies];

            for (int i = 0; i < gameObjects.Length; i++)
            {
                for (int j = 0; j < copies; j++)
                {
                    GameObject instance = Object.Instantiate(gameObjects[i], Vector3.zero, Quaternion.identity);
                    instance.SetActive(false);
                    objects[i * copies + j] = instance;
                }
            }

            size = objects.Length;
        }

        public GameObject Get(Vector3 where)
        {
            if (size == 0)
            {
                return null;
            }

            int index = Random.Range(0, size);
            --size;

            GameObject result = objects[index];
            objects[index] = objects[size];
            objects[size] = result;

            result.transform.position = where;
            result.SetActive(true);

            return result;
        }

        public void Return(GameObject gameObject)
        {
            if (size == objects.Length)
            {
                return;
            }

            gameObject.SetActive(false);
            objects[size] = gameObject;
            ++size;
        }

        public void Clear()
        {
            foreach (GameObject gameObject in objects)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }

    public class RandomPool<T> : IObjectPool<T> where T : MonoBehaviour
    {
        private readonly T[] objects;
        private int size;

        public RandomPool(T[] behaviours, int copies = 1)
        {
            objects = new T[behaviours.Length * copies];

            for (int i = 0; i < behaviours.Length; i++)
            {
                for (int j = 0; j < copies; j++)
                {
                    T instance = Object.Instantiate(behaviours[i], Vector3.zero, Quaternion.identity);
                    instance.gameObject.SetActive(false);

                    objects[i * copies + j] = instance;
                }
            }

            size = objects.Length;
        }

        public T Get(Vector3 where)
        {
            if (size == 0)
            {
                return null;
            }

            int index = Random.Range(0, size);
            --size;

            T result = objects[index];
            objects[index] = objects[size];
            objects[size] = result;

            result.transform.position = where;
            result.gameObject.SetActive(true);

            return result;
        }

        public void Return(T behaviour)
        {
            if (size == objects.Length)
            {
                return;
            }

            behaviour.gameObject.SetActive(false);
            objects[size] = behaviour;
            ++size;
        }

        public void Clear()
        {
            foreach (MonoBehaviour behaviour in objects)
            {
                GameObject.Destroy(behaviour.gameObject);
            }
        }
    }
}