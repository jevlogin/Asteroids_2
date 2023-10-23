using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct AmmunitionStruct
    {
        [SerializeField] private int _poolSize;

        internal int PoolSize => _poolSize;
        
        internal Bullet Bullet;
        internal Pool<Bullet> PoolBullet;
        internal BulletPool PoolBulletGeneric;
    }
}