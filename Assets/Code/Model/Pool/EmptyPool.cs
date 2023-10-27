using System;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class EmptyPool : GenericObjectPool<Asteroid>
    {
        public EmptyPool(Pool<Asteroid> pool, Transform transformParent) : base(pool, transformParent)
        {
        }
    }
}