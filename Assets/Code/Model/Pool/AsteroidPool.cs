using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class AsteroidPool : EmptyPool
    {
        public AsteroidPool(Pool<Asteroid> pool, Transform transformParent) : base(pool, transformParent)
        {
        }

        internal void ExpandThePool(Pool<Asteroid> pool, Asteroid asteroid)
        {
            if (pool.Prefab.AsteroidType == Pool.Prefab.AsteroidType)
            {
                AddObjects(asteroid);
            }
            else
            {
                AddObjects(asteroid);
                ExpandPool(pool, asteroid);
            }
        }
    }
}