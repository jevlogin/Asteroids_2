using System;
using System.Collections.Generic;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct EnemyStruct
    {
        internal Dictionary<AsteroidType, AsteroidPool> PoolsOfType;
        internal Pool<Asteroid> PoolAsteroid;
        internal AsteroidPool PoolAsteroids;

        internal float RadiusSpawnNewEnemy;
    }
}