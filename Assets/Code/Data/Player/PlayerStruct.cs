using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct PlayerStruct
    {
        #region Fields

        [SerializeField] private Speed _speed;
        [SerializeField] private Health _health;
        [SerializeField, Range(0, 1000)] private int _force;

        #endregion


        #region Properties

        public int Force { get => _force; }
        internal Speed Speed { get => _speed; }
        internal Health Health { get => _health; }

        #endregion
    }
}