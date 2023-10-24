using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class AmmunitionSettings
    {
        [SerializeField] private AmmunitionView _bulletPrefab;
        [SerializeField] private string _nameBullet;

        public string NameBullet  => _nameBullet;
        internal AmmunitionView BulletPrefab  => _bulletPrefab;
    }
}