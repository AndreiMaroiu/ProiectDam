using UnityEngine;

namespace Utilities
{
    public class Loader : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(Scenes.LoadAsync(Scenes.MainScene));
        }
    }
}
