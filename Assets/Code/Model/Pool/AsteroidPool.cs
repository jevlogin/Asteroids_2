using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class AsteroidPool : EmptyPool
    {

        public AsteroidPool(Pool<Asteroid> pool, Transform transformParent) : base(pool, transformParent)
        {
        }
    }
}