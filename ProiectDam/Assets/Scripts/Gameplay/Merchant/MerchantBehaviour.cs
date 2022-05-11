using Gameplay.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class MerchantBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform _dialogPosition;
        [SerializeField] private GameObject _dialogCanvas;
        [SerializeField] private Text _dialogText;

        private Animator _animator;

        public void Interact(PlayerController controller)
        {
            string dialogLine = ChooseRandomDialog();
            StartCoroutine(ShowDialog(dialogLine, 2.0f));
            

            _animator.SetBool("StartOpen", true);
            _animator.SetBool("StartHiding", false);
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            StartCoroutine(ShowDialog("Bye!", 1.0f));

            _animator.SetBool("StartHiding", true);
            _animator.SetBool("StartOpen", false);
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private string ChooseRandomDialog()
        {
            int value = Random.Range(0, 10);

            if (value < 5)
            {
                return "Hi!";
            }

            return "Hello!";
        }

        IEnumerator ShowDialog(string dialogText, float openDuration)
        {
            _dialogCanvas.transform.position = _dialogPosition.position;
            _dialogText.text = dialogText;
            _dialogCanvas.SetActive(true);

            yield return new WaitForSeconds(openDuration);

            _dialogCanvas.SetActive(false);
            
        }
    }
}
