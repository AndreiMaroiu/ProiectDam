using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class Loader : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(Scenes.LoadAsync(Scenes.MainScene));
        }
    }
}
