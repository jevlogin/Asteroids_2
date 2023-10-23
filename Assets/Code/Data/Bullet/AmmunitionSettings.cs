using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class AmmunitionSettings
    {
        [SerializeField] private AmmunitionView _bulletPrefab;

        internal AmmunitionView BulletPrefab  => _bulletPrefab;
    }
}