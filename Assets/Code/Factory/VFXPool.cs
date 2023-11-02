using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class VFXPool : GenericObjectPool<ParticleSystem>
    {

        public VFXPool(Pool<ParticleSystem> pool, Transform parent) : base(pool, parent)
        {
        }
    }
}