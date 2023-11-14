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
        [SerializeField] private ParticleSystem _particleCollision;

        public int PoolSize { get => _poolSize; set => _poolSize = value; }
        public ParticleSystem ParticleCollision => _particleCollision; 

        internal IEnumerable<GroupParticle> ExplosionGroup => _explosionGroup;
    }

    [Serializable]
    public class GroupParticle
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private ParticleSystem _particle;

        internal ParticleSystem Particle  => _particle;
        internal EnemyType Type  => _type; 
    }
}