using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            enemyStruct.PoolsOfType = new Dictionary<AsteroidType, AsteroidPool>();
            enemyStruct.RadiusSpawnNewEnemy = _enemyData.EnemySettings.RadiusSpawnEnemy;

            foreach (var enemyGroup in _enemyData.EnemySettings.Enemies)
            {
                var enemy = new GameObject(enemyGroup.Type.ToString());
                enemy.GetOrAddComponent<SpriteRenderer>().sprite = enemyGroup.Sprite;
                enemy.GetOrAddComponent<CircleCollider2D>();

                var enemyView = AddedComponentViewOfTypeObject(ref enemyStruct, enemy, enemyGroup);
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

        private EnemyView AddedComponentViewOfTypeObject(ref EnemyStruct enemyStruct, GameObject enemy, EnemySettingsGroup enemyGroup)
        {
            EnemyView view;
            switch (enemyGroup.Type)
            {
                case AsteroidType.Meteorite:
                case AsteroidType.Cometa:
                    var asteroid = enemy.GetOrAddComponent<Asteroid>();

                    asteroid.Health = new Health(enemyGroup.Health);
                    asteroid.Speed = new Speed(enemyGroup.Speed);
                    asteroid.Damage = enemyGroup.DefaultDamage;
                    asteroid.AsteroidType = enemyGroup.Type;

                    asteroid.BonusPoints = new BonusPoints(_enemyData.EnemySettings.Enemies.FirstOrDefault(e => e.Type == enemyGroup.Type).BonusPoints);

                    enemyStruct.PoolAsteroid = new Pool<Asteroid>(asteroid, enemyGroup.PoolSize);
                    var transformParent = enemyStruct.PoolAsteroids?.TransformParent ?? new GameObject(ManagerName.POOL_ASTEROID).transform;
                    enemyStruct.PoolAsteroids = new AsteroidPool(enemyStruct.PoolAsteroid, transformParent);
                    enemyStruct.PoolAsteroids.OnUpdatePoolAfterAddedNewPoolObjects += PoolAsteroids_OnAddedPool;
                    enemyStruct.PoolAsteroids.AddObjects(asteroid);

                    view = asteroid;
                    break;
                default:
                    view = enemy.GetOrAddComponent<EnemyView>();
                    break;
            }

            return view;
        }
    }
}