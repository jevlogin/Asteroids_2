using System.Collections.Generic;
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

            enemyStruct.PoolsOfType = new Dictionary<EnemyType, EnemyPool>();
            enemyStruct.RadiusSpawnNewEnemy = _enemyData.EnemySettings.RadiusSpawnEnemy;

            foreach (var enemyGroup in _enemyData.EnemySettings.Enemies)
            {
                var enemyView = AddedComponentViewOfTypeObject(ref enemyStruct, enemyGroup);
                components.ListEnemyViews.Add(enemyView);

                enemyStruct.PoolsOfType[enemyGroup.Type] = enemyStruct.PoolEnemy;
            }

            _enemyModel = new EnemyModel(enemyStruct, components, settings);
            return _enemyModel;
        }

        private void PoolAsteroids_OnAddedPool(List<Asteroid> list, Asteroid asteroid)
        {
            foreach (var item in list)
            {
                item.Type = asteroid.Type;
            }
        }

        private EnemyView AddedComponentViewOfTypeObject(ref EnemyStruct enemyStruct, EnemySettingsGroup enemyGroup)
        {
            EnemyView view = null;
            Transform transformParent = enemyStruct.PoolEnemy?.TransformParent;

            var enemy = Object.Instantiate(enemyGroup.PrefabEnemy);

            enemy.name = enemyGroup.PrefabEnemy.name;
            switch (enemyGroup.Type)
            {
                case EnemyType.Ship:
                    var ship = enemy.gameObject.GetOrAddComponent<Ship>();
                    ship.Health = new Health(enemyGroup.Health);
                    ship.Speed = new Speed(enemyGroup.Speed);
                    ship.Damage = enemyGroup.DefaultDamage;
                    ship.Type = enemyGroup.Type;
                    ship.BonusPoints = new BonusPoints(enemyGroup.BonusPoints);
                    ship.ExpirienceAfterDead = enemyGroup.Expirience;

                    var poolShip = new Pool<EnemyView>(ship, enemyGroup.PoolSize);
                    transformParent ??= new GameObject(ManagerName.POOL_ENEMY).transform;
                    
                    enemyStruct.PoolEnemy = new EnemyPool(poolShip, transformParent, enemyGroup);

                    enemyStruct.PoolEnemy.AddObjects(ship);

                    view = ship;
                    break;
                case EnemyType.Meteorite:
                case EnemyType.Cometa:

                    var asteroid = enemy.gameObject.GetOrAddComponent<Asteroid>();
                    asteroid.Rigidbody = asteroid.gameObject.GetOrAddComponent<Rigidbody2D>();

                    asteroid.Health = new Health(enemyGroup.Health);
                    asteroid.Speed = new Speed(enemyGroup.Speed);
                    asteroid.Damage = enemyGroup.DefaultDamage;
                    asteroid.Type = enemyGroup.Type;
                    asteroid.BonusPoints = new BonusPoints(enemyGroup.BonusPoints);
                    asteroid.ExpirienceAfterDead = enemyGroup.Expirience;

                    transformParent ??= new GameObject(ManagerName.POOL_ASTEROID).transform;
                    var pool = new Pool<EnemyView>(asteroid, enemyGroup.PoolSize);
                    enemyStruct.PoolEnemy = new EnemyPool(pool, transformParent, enemyGroup);
                    enemyStruct.PoolEnemy.AddObjects(asteroid);

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