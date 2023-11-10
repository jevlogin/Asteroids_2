using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class AsteroidPool : EmptyPoolAsteroid
    {
        internal EnemySettingsGroup EnemySettingsGroup;

        public AsteroidPool(Pool<Asteroid> pool, Transform transformParent, EnemySettingsGroup enemySettingsGroup) : base(pool, transformParent)
        {
            EnemySettingsGroup = enemySettingsGroup;
        }
    }
}