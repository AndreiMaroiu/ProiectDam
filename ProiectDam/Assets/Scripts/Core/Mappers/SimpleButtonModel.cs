using UnityEngine;
using UnityEngine.Events;

namespace Core.Mappers
{
    public class SimpleButtonModel : IButtonModel
    {
        public SimpleButtonModel(string text, UnityAction action, GameObject owner)
        {
            ButtonText = text;
            Action = action;
            Owner = owner;
        }

        public UnityAction Action { get; }

        public string ButtonText { get; }

        public GameObject Owner { get; }
    }
}
