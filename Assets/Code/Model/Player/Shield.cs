using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class Shield
    {
        [SerializeField, Range(0, 100)] private float _currentValue;
        [SerializeField, Range(20, 100)] private int _maxValue;

        public float MaxValue { get => _maxValue; set => _maxValue = (int)value; }
        public float CurrentValue { get => _currentValue; set => _currentValue = value; }

        public Shield(Shield health)
        {
            MaxValue = health.MaxValue;
            CurrentValue = health.CurrentValue;
        }
    }
}
