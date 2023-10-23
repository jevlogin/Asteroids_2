using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct AmmunitionStruct
    {
        [SerializeField] private Bullet _bullet;

        internal Bullet Bullet => _bullet;
    }
}