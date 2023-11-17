using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Health
    {
        internal event Action<Health> OnChangeHealth;

        [SerializeField, Range(20, 100)] private int _maxHealth;
        [SerializeField, Range(0, 100)] private float _currentHealth;

        [SerializeField, Range(0, 1), Header("Процентное соотношение 0.1 = 10%")] private float _percent;
        private float _valuePercentUpdate;


        public float PercentUpdate => _percent;

        public float ValuePercentUpdate => _valuePercentUpdate;
        
        public float MaxHealth
        {
            get => _maxHealth; 
            set
            {
                _maxHealth = Mathf.RoundToInt(value);
                _valuePercentUpdate = Mathf.RoundToInt(_maxHealth * PercentUpdate);
                _currentHealth = _maxHealth;
                OnChangeHealth?.Invoke(this);
            }
        }
        public float CurrentHealth
        {
            get => _currentHealth; 
            set
            {
                _currentHealth = value;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
                _currentHealth = Mathf.RoundToInt(_currentHealth);
                OnChangeHealth?.Invoke(this);
            }
        }


        public Health(Health health)
        {
            _percent = health.PercentUpdate;
            CurrentHealth = health.CurrentHealth;
            MaxHealth = health.MaxHealth;
        }
    }
}