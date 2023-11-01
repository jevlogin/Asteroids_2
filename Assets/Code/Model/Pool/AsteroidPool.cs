using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class AsteroidPool : EmptyPoolAsteroid
    {
        public AsteroidPool(Pool<Asteroid> pool, Transform transformParent) : base(pool, transformParent)
        {
        }
    }
}