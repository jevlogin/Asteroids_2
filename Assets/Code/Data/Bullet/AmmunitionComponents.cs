using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class AmmunitionComponents
    {
        public AmmunitionView BulletView;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
    }
}