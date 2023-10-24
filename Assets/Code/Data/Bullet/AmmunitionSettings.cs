using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class AmmunitionSettings
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private string _nameBullet;

        public string NameBullet  => _nameBullet;
        internal Bullet BulletPrefab  => _bulletPrefab;
    }
}