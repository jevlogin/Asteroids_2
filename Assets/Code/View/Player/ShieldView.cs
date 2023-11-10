using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class ShieldView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _colliderParticle;
        [SerializeField] private ParticleSystem _shieldParticle;

        public ParticleSystem ColliderParticle { get => _colliderParticle; set => _colliderParticle = value; }
        public ParticleSystem ShieldParticle { get => _shieldParticle; set => _shieldParticle = value; }

        internal void PlayShield()
        {
            if (!_colliderParticle.isPlaying)
            {
                _colliderParticle.Play(); 
            }
            else
            {
                _colliderParticle.Stop();
                _colliderParticle.Play();
            }
        }

        internal void StopAllParticles()
        {
            _colliderParticle.Stop();
            _shieldParticle.Stop();
        }
    }
}