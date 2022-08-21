using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class Loader : MonoBehaviour
    {
        public static int TargetScene { get; set; } = Scenes.MainScene;

        [SerializeField] private Animator _transition;

        private IEnumerator Start()
        {
            // wait for fade in animation to finish
            yield return new WaitForSeconds(1.0f);

            AsyncOperation loader = SceneManager.LoadSceneAsync(TargetScene);
            loader.allowSceneActivation = false;

            while (!loader.isDone)
            {
                if (loader.progress >= 0.9f) // before loading new scene, play fade out animation
                {
                    _transition.SetTrigger("Start");
                    yield return new WaitForSeconds(1.0f);

                    loader.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
