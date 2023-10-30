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
        public ParticleSystem Particles;
    }
}