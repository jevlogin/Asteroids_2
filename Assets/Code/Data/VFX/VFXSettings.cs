using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class VFXSettings
    {
        [SerializeField] private List<GroupParticle> _explosionGroup;
        [SerializeField] private int _poolSize;

        public int PoolSize { get => _poolSize; set => _poolSize = value; }

        internal IEnumerable<GroupParticle> ExplosionGroup => _explosionGroup;
    }

    [Serializable]
    public class GroupParticle
    {
        [SerializeField] private AsteroidType _type;
        [SerializeField] private ParticleSystem _particle;

        internal ParticleSystem Particle  => _particle;
        internal AsteroidType Type  => _type; 
    }
}