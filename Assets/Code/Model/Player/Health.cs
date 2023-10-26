using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Health
    {
        [SerializeField, Range(20, 100)] private int _maxHealth;
        [SerializeField, Range(0, 100)] private float _currentHealth;

        public float MaxHealth { get => _maxHealth; set => _maxHealth = (int)value; }
        public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

        public Health(Health health)
        {
            MaxHealth = health.MaxHealth;
            CurrentHealth = health.CurrentHealth;
        }
    }
}