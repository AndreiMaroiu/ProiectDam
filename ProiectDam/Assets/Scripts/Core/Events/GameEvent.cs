using System;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Scriptables/Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        public event Action<object> OnEvent;

        public void Invoke(object sender) => OnEvent?.Invoke(sender);
    }
}
