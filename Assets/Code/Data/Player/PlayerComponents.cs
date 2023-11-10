using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class PlayerComponents
    {
        public Transform PlayerTransform;
        public Transform BarrelTransform;
        public Rigidbody RigidbodyEnergyBlock;
        public Rigidbody2D RigidbodyPlayer;
        public PlayerView PlayerView;
        [Header("Stars System")] public ParticleSystem ParticlesStarSystem;
        public AudioSource AudioSource;
        public ShieldView ShieldView;
    }
}