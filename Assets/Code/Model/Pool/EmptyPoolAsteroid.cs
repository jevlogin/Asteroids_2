using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class EmptyPoolAsteroid : GenericObjectPool<Asteroid>
    {
        public EmptyPoolAsteroid(Pool<Asteroid> pool, Transform transformParent) : base(pool, transformParent)
        {
        }
    }
}