using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class BackgroundPool
    {
        private Pool<ParticleSystem> pool;
        private Transform transformPoolParent;

        public BackgroundPool(Pool<ParticleSystem> pool, Transform transformPoolParent)
        {
            this.pool = pool;
            this.transformPoolParent = transformPoolParent;
        }
    }
}