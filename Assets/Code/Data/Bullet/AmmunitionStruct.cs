using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct AmmunitionStruct
    {
        [SerializeField] private int _poolSize;
        [SerializeField] private float _refireTimer;
        [SerializeField] private AmmunitionType _typeAmmunition;


        internal Bullet Bullet;
        internal Pool<Bullet> PoolBullet;
        internal BulletPool PoolBulletGeneric;

        internal AmmunitionType AmmunitionType => _typeAmmunition;
        internal float RefireTimer { get => _refireTimer; private set => _refireTimer = value; }
        internal readonly int PoolSize => _poolSize;
    }
}