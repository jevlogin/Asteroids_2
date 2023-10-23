using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Health
    {
        [SerializeField, Range(20, 100)] private int _maxHealth;
        [SerializeField, Range(0, 100)] private float _currentHealth;

        public float MaxHealth { get => _maxHealth; }
        public float CurrentHealth { get => (float)_currentHealth; }

        public Health(float maxHealth, float currentHealth)
        {
            _maxHealth = (int)maxHealth;
            _currentHealth = currentHealth;
        }
    }
}