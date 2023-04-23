using UnityEngine.SceneManagement;

namespace Core.Services
{
    public interface ISceneAwareService
    {
        void OnSceneLoaded(Scene scene);
        void OnSceneUnloaded(Scene scene);
    }
}
