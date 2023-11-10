using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

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

            enemyStruct.PoolsOfType = new Dictionary<EnemyType, AsteroidPool>();
            enemyStruct.RadiusSpawnNewEnemy = _enemyData.EnemySettings.RadiusSpawnEnemy;

            foreach (var enemyGroup in _enemyData.EnemySettings.Enemies)
            {
                var enemyView = AddedComponentViewOfTypeObject(ref enemyStruct, enemyGroup);
                components.ListEnemyViews.Add(enemyView);

                enemyStruct.PoolsOfType[enemyGroup.Type] = enemyStruct.PoolAsteroids;
            }

            _enemyModel = new EnemyModel(enemyStruct, components, settings);
            return _enemyModel;
        }

        private void PoolAsteroids_OnAddedPool(List<Asteroid> list, Asteroid asteroid)
        {
            foreach (var item in list)
            {
                item.AsteroidType = asteroid.AsteroidType;
            }
        }

        private EnemyView AddedComponentViewOfTypeObject(ref EnemyStruct enemyStruct, EnemySettingsGroup enemyGroup)
        {
            EnemyView view = null;

            var enemy = Object.Instantiate(enemyGroup.PrefabEnemy);

            switch (enemyGroup.Type)
            {
                case EnemyType.Ship:

                    break;
                case EnemyType.Meteorite:
                case EnemyType.Cometa:

                    var asteroid = enemy.gameObject.GetOrAddComponent<Asteroid>();
                    asteroid.Rigidbody = asteroid.gameObject.GetOrAddComponent<Rigidbody2D>();

                    asteroid.Health = new Health(enemyGroup.Health);
                    asteroid.Speed = new Speed(enemyGroup.Speed);
                    asteroid.Damage = enemyGroup.DefaultDamage;
                    asteroid.AsteroidType = enemyGroup.Type;

                    asteroid.BonusPoints = new BonusPoints(_enemyData.EnemySettings.Enemies.FirstOrDefault(e => e.Type == enemyGroup.Type).BonusPoints);

                    enemyStruct.PoolAsteroid = new Pool<Asteroid>(asteroid, enemyGroup.PoolSize);
                    var transformParent = enemyStruct.PoolAsteroids?.TransformParent ?? new GameObject(ManagerName.POOL_ASTEROID).transform;
                    enemyStruct.PoolAsteroids = new AsteroidPool(enemyStruct.PoolAsteroid, transformParent, enemyGroup);
                    enemyStruct.PoolAsteroids.OnUpdatePoolAfterAddedNewPoolObjects += PoolAsteroids_OnAddedPool;
                    enemyStruct.PoolAsteroids.AddObjects(asteroid);

                    view = asteroid;
                    break;
                default:
                    view = enemy;
                    Debug.LogWarning($"Enemy View is not changed {nameof(view)}");
                    break;
            }

            return view;
        }
    }
}