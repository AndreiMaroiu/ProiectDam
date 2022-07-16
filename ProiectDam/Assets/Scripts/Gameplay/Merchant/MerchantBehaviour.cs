using Gameplay.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Merchant
{
    public class MerchantBehaviour : PressableObject, IInteractable
    {
        [SerializeField] private Transform _dialogPosition;
        [SerializeField] private GameObject _dialogCanvas;
        [SerializeField] private Text _dialogText;
        [SerializeField] private VerticalLayoutGroup _layout;
        [SerializeField] private ShopItems _items;

        private Animator _animator;
        private PlayerController _player;

        public void Interact(PlayerController controller)
        {
            _player = controller;

            string dialogLine = ChooseRandomDialog();
            StartCoroutine(ShowDialog(dialogLine, 2.0f));
            
            _animator.SetTrigger("Open");
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            StartCoroutine(ShowDialog("Bye!", 1.0f));

            _animator.SetTrigger("Hide");
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _dialogCanvas.SetActive(false);
        }

        private string ChooseRandomDialog()
        {
            int value = Random.Range(0, 10);

            //if (value < 5)
            //{
            //    return "Hi!";
            //}

            //return "Hello!";
            return "Hello\nHi!";
        }

        IEnumerator ShowDialog(string dialogText, float openDuration)
        {
            //StopAllCoroutines();

            _dialogCanvas.SetActive(true);
            _dialogCanvas.transform.position = _dialogPosition.position;
            _dialogText.text = dialogText;
            _layout.enabled = false;

            yield return null;
            _layout.enabled = true;
            

            yield return new WaitForSeconds(openDuration);

            _dialogCanvas.SetActive(false);
        }

        public override void OnClick()
        {
            _items[0].GetPickUp().Interact(_player);
        }
    }
}
