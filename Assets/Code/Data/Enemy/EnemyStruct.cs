using System;
using System.Collections.Generic;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct EnemyStruct
    {
        internal Dictionary<EnemyType, EnemyPool> PoolsOfType;
        internal Pool<Asteroid> PoolAsteroid;
        internal AsteroidPool PoolAsteroids;
        internal EnemyPool PoolEnemy;

        internal float RadiusSpawnNewEnemy;
    }
}