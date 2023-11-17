using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class Expirience
    {
        [SerializeField] private float _currentValue;
        [SerializeField] private int _maxValue;
        private int _currentLevel = 0;
        private int _freePoints = 0;

        internal event Action<Expirience> OnChangeExpirience;
        internal event Action<Expirience> OnLevelUp;
        internal event Action<int> OnChangeFreePoints;

        private readonly float _baseValueExpirience;
        private readonly float _multiplierExpirience;

        public float MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = Mathf.RoundToInt(value);
            }
        }

        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = Mathf.RoundToInt(Mathf.Clamp(value, 0, _maxValue));
                if (_currentValue == _maxValue)
                {
                    _currentValue = 0;
                    CurrentLevel++;
                    MaxValue = CalculateMaxExperience(_currentLevel);
                }
                OnChangeExpirience?.Invoke(this);
            }
        }

        public int CurrentLevel
        {
            get => _currentLevel; 
            set
            {
                _currentLevel = value;
                FreePoints++;
                OnChangeExpirience?.Invoke(this);
                OnLevelUp?.Invoke(this);
            }
        }

        internal int FreePoints
        {
            get => _freePoints; 
            set
            {
                _freePoints = Math.Max(value, 0);
                OnChangeFreePoints?.Invoke(_freePoints);
            }
        }
      
        public Expirience(int currentLevel, float multiplierExpirience, float baseValueExpirience)
        {
            _currentLevel = currentLevel;
            _baseValueExpirience = baseValueExpirience;
            _multiplierExpirience = multiplierExpirience;

            MaxValue = CalculateMaxExperience(currentLevel);
            CurrentValue = 0;
        }

        private int CalculateMaxExperience(int currentLevel)
        {
            return Mathf.RoundToInt(_baseValueExpirience * Mathf.Pow(_multiplierExpirience, currentLevel - 1));
        }
    }
}
