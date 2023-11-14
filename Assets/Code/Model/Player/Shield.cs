using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class Shield
    {
        [SerializeField, Range(0, 100)] private float _currentValue;
        [SerializeField, Range(20, 100)] private int _maxValue;
        internal event Action<Shield> OnChangeShield;

        public float MaxValue
        {
            get => _maxValue; set
            {
                _maxValue = (int)value;
                OnChangeShield?.Invoke(this);
            }
        }
        public float CurrentValue
        {
            get => _currentValue; set
            {
                _currentValue = value;
                OnChangeShield?.Invoke(this);
            }
        }

        public Shield(Shield health)
        {
            MaxValue = health.MaxValue;
            CurrentValue = health.CurrentValue;
        }
    }
}
