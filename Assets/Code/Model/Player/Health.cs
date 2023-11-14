using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Health
    {
        [SerializeField, Range(20, 100)] private int _maxHealth;
        [SerializeField, Range(0, 100)] private float _currentHealth;

        internal event Action<Health> OnChangeHealth;

        public float MaxHealth
        {
            get => _maxHealth; set
            {
                _maxHealth = (int)value;
                OnChangeHealth?.Invoke(this);
            }
        }
        public float CurrentHealth
        {
            get => _currentHealth; set
            {
                _currentHealth = value;
                OnChangeHealth?.Invoke(this);
            }
        }

        public Health(Health health)
        {
            MaxHealth = health.MaxHealth;
            CurrentHealth = health.CurrentHealth;
        }
    }
}