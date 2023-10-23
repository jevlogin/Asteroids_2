using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class BulletPool : GenericObjectPool<Bullet>
    {
        public BulletPool(Pool<Bullet> pool, Transform transformParent) : base(pool, transformParent)
        {
        }
    }
}