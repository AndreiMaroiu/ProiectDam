using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class Loader : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1.0f);
            yield return Scenes.LoadAsync(Scenes.MainScene);
        }
    }
}
