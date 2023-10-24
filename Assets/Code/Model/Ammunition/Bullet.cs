using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Bullet : AmmunitionView
    {
        [SerializeField, Range(0, 10)] private float _maxLifetimeOutsideThePool;

        public float LifeTime { get; set; } = 0.0f;

        public float MaxLifeTimeOutsideThePool
        {
            get
            {
                return _maxLifetimeOutsideThePool;
            }
        }
    }
}