using Core.Values;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Hub
{
    public class HubPointDescription : MonoBehaviour, IHubPointListener
    {
        [SerializeField] private HubPointDescriptionEvent _descriptionEvent;
        [SerializeField, TextArea] private string _description;

        public void OnEnter()
        {
            _descriptionEvent.Value = _description;
        }

        public void OnExit()
        {
            _descriptionEvent.Value = null;
        }
    }
}
