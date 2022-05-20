using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private Animator _transition;

        private IEnumerator Start()
        {
            // wait for fade in animation to finish
            yield return new WaitForSeconds(1.0f);

            AsyncOperation loader = SceneManager.LoadSceneAsync(Scenes.MainScene);
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
