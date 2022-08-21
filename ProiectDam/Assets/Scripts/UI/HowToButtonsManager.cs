using UnityEngine;

namespace UI
{
    public class HowToButtonsManager : MonoBehaviour
    {
        [SerializeField] private bool _showTutorialButton;
        [Header("Buttons")]
        [SerializeField] private GameObject _tutotialButton;

        private void Start()
        {
            _tutotialButton.SetActive(_showTutorialButton);
        }
    }
}
