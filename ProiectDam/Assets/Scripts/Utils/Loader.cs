using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private Animator _transition;
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1.0f);
            _transition.SetTrigger("Start");
            yield return Scenes.LoadAsync(Scenes.MainScene);
        }
    }
}
