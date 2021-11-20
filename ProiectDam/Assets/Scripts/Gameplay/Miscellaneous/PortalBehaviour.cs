using Gameplay.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class PortalBehaviour : MonoBehaviour, IInteractable
    {
        Coroutine _coroutine = null;

        public void Interact(PlayerController controller)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(FinishGame());
        }

        IEnumerator FinishGame()
        {
            yield return new WaitForSeconds(2.0f);

            float timeScale = Time.timeScale;
            Time.timeScale = 0;
            ModalWindows.ModalWindow.ShowSimpleDialog("You Won", () =>
            {
                Time.timeScale = timeScale;
                SceneManager.LoadScene(1);
            });
        }
    }
}
