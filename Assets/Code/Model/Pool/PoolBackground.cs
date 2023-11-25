using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class PoolBackground : GenericObjectPool<BackgroundView>, IBackgroundPool
    {
        public PoolBackground(Pool<BackgroundView> poolPlanet, Transform transformPoolParent) : base(poolPlanet, transformPoolParent)
        {
        }
    }
}