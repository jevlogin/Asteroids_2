using System;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public class DelayExplosionObject : DelayGenericObject<ParticleSystem>
    {
        public EnemyType Type;

        public DelayExplosionObject(ParticleSystem source, float length, EnemyType type) : base(source, length)
        {
            Type = type;
        }
    }
}