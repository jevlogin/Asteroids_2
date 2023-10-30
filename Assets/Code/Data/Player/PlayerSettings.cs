using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class PlayerSettings
    {
        #region Fields

        [SerializeField] private Speed _speed;
        [SerializeField] private Health _health;
        [SerializeField, Range(0, 1000)] private int _force;
        [SerializeField, Range(0, 100)] private float _damage;

        [SerializeField, Header("Prefab player")] private Player _playerView;
        
        [SerializeField, Header("Система частиц для корабля"), Space(20)] private GameObject _particleSystem;

        [SerializeField, Space(10), Header("Вектор смещения для ствола пушки"), Space(20)] private Vector2 _offsetVectorBullet;

        internal Vector3 TransformPositionEnergyBlock;
        #endregion


        #region Properties

        internal Player PlayerPrefab => _playerView;
        internal float Damage => _damage;
        internal int Force => _force;
        internal Speed Speed => _speed;
        internal Health Health => _health;
        internal GameObject ParticleSystem => _particleSystem;
        internal Vector2 OffsetVectorBurel => _offsetVectorBullet;

        #endregion
    }
}