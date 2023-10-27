using System;
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

            enemyStruct.PoolsOfType = new Dictionary<AsteroidType, AsteroidPool>();

            foreach (var enemyGroup in _enemyData.EnemySettings.Enemies)
            {
                var enemy = new GameObject(enemyGroup.Type.ToString());
                enemy.GetOrAddComponent<SpriteRenderer>().sprite = enemyGroup.Sprite;
                enemy.GetOrAddComponent<CircleCollider2D>();

                var enemyView = AddedComponentViewOfTypeObject(ref enemyStruct, enemy, enemyGroup);
                components.ListEnemyViews.Add(enemyView);

                enemyStruct.PoolsOfType[enemyGroup.Type] = enemyStruct.PoolAsteroids;

                Debug.Log($"enemyView = {(enemyView as Asteroid).AsteroidType}");

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

                    enemyStruct.PoolAsteroid = new Pool<Asteroid>(asteroid, enemyGroup.PoolSize);

                    var transformParent = enemyStruct.PoolAsteroids?.TransformParent ?? new GameObject(ManagerName.POOL_ASTEROID).transform;

                    enemyStruct.PoolAsteroids = new AsteroidPool(enemyStruct.PoolAsteroid, transformParent);
                    enemyStruct.PoolAsteroids.OnAddedPool += PoolAsteroids_OnAddedPool;
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