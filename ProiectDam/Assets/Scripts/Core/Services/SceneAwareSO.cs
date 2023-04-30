using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public abstract class SceneAwareSO : ScriptableObject
    {
        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            SceneManager.sceneUnloaded -= SceneUnloaded;
        }

        protected abstract void SceneUnloaded(Scene scene);

        protected abstract void SceneLoaded(Scene scene, LoadSceneMode mode);
    }
}
