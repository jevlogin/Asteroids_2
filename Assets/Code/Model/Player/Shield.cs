using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class Shield
    {
        [SerializeField, Range(0, 100)] private float _currentValue;
        [SerializeField, Range(20, 100)] private int _maxValue;
        [SerializeField, Range(0, 1)] internal float DamageAbsorptionCoefficient;

        internal event Action<Shield> OnChangeShield;

        private float _shieldRecoveryTime = 5.0f;
        internal bool IsRemaining;

        internal float ShieldRecoveryTime => _shieldRecoveryTime;
        internal float TimeSinceLastDamage;

        public float MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = Mathf.Max((int)value, 0);
                OnChangeShield?.Invoke(this);
            }
        }
        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = Mathf.Max(value, 0);
                _currentValue = Mathf.RoundToInt(_currentValue);
                
                if (_currentValue >= MaxValue)
                    IsRemaining = false;
                
                OnChangeShield?.Invoke(this);
            }
        }

        public Shield(Shield health)
        {
            MaxValue = health.MaxValue;
            CurrentValue = health.CurrentValue;
            DamageAbsorptionCoefficient = health.DamageAbsorptionCoefficient;
        }

        internal void ResetLastDamage()
        {
            TimeSinceLastDamage = 0f;
            IsRemaining = true;
        }
    }
}
