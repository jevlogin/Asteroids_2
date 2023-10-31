using System;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct ConfigParticlesShip
    {
        public float StartLifetime;
        public float StartSpeed;
        public Color StartColor;
        public bool IsPlayOnAwake;
    }
}