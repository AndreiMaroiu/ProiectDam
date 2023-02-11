using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public static class Scenes
    {
        public const int MainScene = 1;
        public const int MainMenu = 0;
        public const int LoadingMenu = 2;
        public const int Tutorial = 3;
        public const int Hub = 4;

        public static IEnumerator LoadAsync(int scene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
