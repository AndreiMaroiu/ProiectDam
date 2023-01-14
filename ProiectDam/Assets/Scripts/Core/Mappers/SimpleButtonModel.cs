using UnityEngine.Events;

namespace Core.Mappers
{
    public class SimpleButtonModel : IButtonModel
    {
        public SimpleButtonModel(string text, UnityAction action)
        {
            ButtonText = text;
            Action = action;
        }

        public UnityAction Action { get; }

        public string ButtonText { get; }
    }
}
