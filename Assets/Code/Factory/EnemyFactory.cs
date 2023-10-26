using System.Collections.Generic;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class EnemyFactory
    {
        private EnemyData _enemyData;
        private EnemyModel _enemyModel;

        public EnemyFactory(EnemyData enemyData)
        {
            _enemyData = enemyData;
        }

        internal EnemyModel GetOrCreateEnemyModel()
        {
            var enemyStruct = _enemyData.EnemyStruct;
            var components = new EnemyComponents();
            var settings = new EnemySettings();

            enemyStruct.PoolAsteroidsList = new List<AsteroidPool>();
            enemyStruct.PoolsOfType = new Dictionary<AsteroidType, AsteroidPool>();

            foreach (var enemyGroup in _enemyData.EnemySettings.Enemies)
            {
                var enemy = new GameObject(enemyGroup.Type.ToString());
                var id = enemy.GetInstanceID();
                enemy.GetOrAddComponent<SpriteRenderer>().sprite = enemyGroup.Sprite;
                enemy.GetOrAddComponent<CircleCollider2D>();

                var enemyView = AddedComponentViewOfTypeObject(components, enemy, id, enemyGroup.Type);

                if (enemyView is Asteroid asteroid)
                {
                    asteroid.Health = new Health(enemyGroup.Health);
                    asteroid.Speed = new Speed(enemyGroup.Speed);
                    asteroid.Damage = enemyGroup.DefaultDamage;
                    asteroid.AsteroidType = enemyGroup.Type;

                    enemyStruct.PoolAsteroid = new Pool<Asteroid>(asteroid, enemyGroup.PoolSize);

                    if (enemyStruct.PoolAsteroids == null)
                    {
                        enemyStruct.PoolAsteroids = new AsteroidPool(enemyStruct.PoolAsteroid, new GameObject(ManagerName.POOL_ASTEROID).transform);

                        enemyStruct.PoolAsteroids.AddObjects(asteroid);
                    }
                    else
                    {
                        enemyStruct.PoolAsteroids.ExpandThePool(enemyStruct.PoolAsteroid, asteroid);
                    }
                }
            }
            enemyStruct.PoolAsteroidsList.Add(enemyStruct.PoolAsteroids);

            _enemyModel = new EnemyModel(enemyStruct, components, settings);

            return _enemyModel;
        }

        private static EnemyView AddedComponentViewOfTypeObject(EnemyComponents components, GameObject enemy, int id, AsteroidType type)
        {
            EnemyView view;
            switch (type)
            {
                case AsteroidType.Meteorite:
                case AsteroidType.Cometa:
                    view = enemy.GetOrAddComponent<Asteroid>();
                    break;
                default:
                    view = enemy.GetOrAddComponent<EnemyView>();
                    break;
            }

            return view;
        }
    }
}