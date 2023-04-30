using System;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public interface ISceneAwareService
    {
        void OnSceneLoaded(Scene scene);
        void OnSceneUnloaded(Scene scene);

        // should be triggered on loaded
        event Action ServiceLoaded;
    }
}
