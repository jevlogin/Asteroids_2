using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class PlayerSettings
    {
        #region Fields

        [SerializeField] private float _timeForShipToTakeOff;
        [SerializeField] private float _speedAtTakeShip;
        [SerializeField] private Speed _speed;
        [SerializeField] private Health _health;
        [SerializeField, Range(0, 1000)] private int _force;
        [SerializeField, Range(0, 100)] private float _damage;
        [SerializeField, Range(0, 1), Header("Громкость источника звука")] private float _audioSourceVolume;

        [SerializeField, Header("Prefab player")] private Player _playerView;

        [SerializeField, Header("Система частиц для корабля"), Space(20)] private GameObject _particleSystem;
        [SerializeField, Header("Система частиц Щит"), Space(20)] private ShieldView _particleSystemShield;
        [SerializeField, Header("Стартовый размер щита")] private float _shieldStartSize;
        [SerializeField, Header("Стартовая позиция щита")] private Vector3 _shieldStartPosition;

        [SerializeField, Space(10), Header("Вектор смещения для ствола пушки"), Space(20)] private Vector2 _offsetVectorBullet;

        internal Vector3 TransformPositionEnergyBlock;

        [SerializeField, Header("Настройки для системы частиц корабля")] internal ConfigParticlesShip ConfigParticlesShip;
        [SerializeField, Header("Настройки для системы частиц корабля")] internal ConfigParticlesShip ConfigParticlesShipDefault;

        #endregion


        #region Properties

        internal Player PlayerPrefab => _playerView;
        internal float Damage => _damage;
        internal int Force => _force;
        internal Speed Speed => _speed;
        internal Health Health => _health;
        internal GameObject ParticleSystem => _particleSystem;
        internal ShieldView ParticleSystemShield => _particleSystemShield;
        internal Vector2 OffsetVectorBurel => _offsetVectorBullet;
        public float TimeForShipToTakeOff { get => _timeForShipToTakeOff; set => _timeForShipToTakeOff = value; }
        public float SpeedAtTakeShip { get => _speedAtTakeShip; set => _speedAtTakeShip = value; }
        public float AudioSourceVolume => _audioSourceVolume;
        public float ShieldStartSize  => _shieldStartSize; 
        public Vector3 ShieldStartPosition => _shieldStartPosition; 

        #endregion
    }
}