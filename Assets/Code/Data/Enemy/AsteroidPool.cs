using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class AsteroidPool : GenericObjectPool<Asteroid>
    {
        public AsteroidPool(Pool<Asteroid> pool, Transform transformParent) : base(pool, transformParent)
        {
        }
    }
}