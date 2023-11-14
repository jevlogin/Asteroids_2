using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct VFXStruct
    {
        internal Dictionary<EnemyType, ParticleSystem> ExplosionEffects;

        internal Dictionary<EnemyType, VFXPool> PoolsVFX;

        internal ParticleSystem ParticleCollision;
        public Transform TransformParent;
    }
}