using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct VFXStruct
    {
        internal Dictionary<AsteroidType, ParticleSystem> ExplosionEffects;

        internal Dictionary<AsteroidType, VFXPool> PoolsVFX;

        public Transform TransformParent;
    }
}