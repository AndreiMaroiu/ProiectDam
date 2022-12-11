using Core.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PanelsManager : MonoBehaviour
    {
        [SerializeField] GameObject _pausePanel;
        [SerializeField] BoolEvent _panelEvent;

        private void Start()
        {
        
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape) && _panelEvent.Value is false)
            {
                _pausePanel.SetActive(true);
            }
        }
    }
}
