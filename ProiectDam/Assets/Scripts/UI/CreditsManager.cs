using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CreditsManager : MonoBehaviour
    {
        [SerializeField] private string URL;
        private void OnClick()
        {
            Application.OpenURL(URL);
        }
    }
}
