using System;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public class DelayExplosionObject : DelayGenericObject<ParticleSystem>
    {
        public AsteroidType Type;

        public DelayExplosionObject(ParticleSystem source, float length, AsteroidType type) : base(source, length)
        {
            Type = type;
        }
    }
}