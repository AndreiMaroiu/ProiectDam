using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "New Health Event", menuName = "Scriptables/Events/Health Event")]
    public class HealthEvent : ScriptableObject
    {
        public event Action OnHealthChanged;
        public event Action OnMaxHealthChanged;

        private int _health;
        private int _maxHealth;

        public int Health
        {
            get => _health;

            set
            {
                _health = value;

                OnHealthChanged?.Invoke();
            }
        }

        public int MaxHealth
        {
            get => _maxHealth;

            set
            {
                _maxHealth = value;

                OnMaxHealthChanged?.Invoke();
            }
        }
    }
}
