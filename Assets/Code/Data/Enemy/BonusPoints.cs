using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class BonusPoints
    {
        [SerializeField, Range(0.0f, 3.0f)] private int _bonusPointsMin;
        [SerializeField, Range(5.0f, 10.0f)] private int _bonusPointsMax;
        private int _bonus;
        private bool _isAssignedBonusPoints;

        public BonusPoints(BonusPoints bonusPoints)
        {
            _bonusPointsMin = bonusPoints._bonusPointsMin;
            _bonusPointsMax = bonusPoints._bonusPointsMax;
        }
        
        internal int BonusPointsAfterDeath
        {
            get
            {
                if (!_isAssignedBonusPoints)
                {
                    _bonus = Random.Range(_bonusPointsMin, _bonusPointsMax);
                    _isAssignedBonusPoints = true;
                }
                return _bonus;
            }
        }
    }
}