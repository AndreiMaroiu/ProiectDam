using Core.Events;
using Gameplay.Player;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class PortalBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameEvent _winEvent;

        private Coroutine _coroutine = null;

        public void OnInteract(PlayerController controller)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(FinishGame());
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        IEnumerator FinishGame()
        {
            yield return new WaitForSeconds(2.0f);

            _winEvent.Invoke();
        }
    }
}
