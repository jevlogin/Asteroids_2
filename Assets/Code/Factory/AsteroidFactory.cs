using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class AsteroidFactory : IEnemyFactory
    {
        public EnemyView Create(Health health)
        {
            var enemy = Object.Instantiate(Resources.Load<Asteroid>("Data/Enemy/Asteroid"));

            return enemy;
        }
    }
}