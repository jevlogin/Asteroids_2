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
        public PlayerView PlayerView;
        [Header("Stars System")] public ParticleSystem Particles;
        public AudioSource AudioSource;
    }
}