using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace WORLDGAMEDEVELOPMENT
{
    internal class EnemyController : IController, ICleanup, IInitialization, IEventActionGeneric<float>
    {
        private EnemyModel _enemyModel;
        private readonly PlayerModel _playerModel;
        private bool _isStopControl;
        private List<Asteroid> _activeEnemyList;
        private readonly SceneController _sceneController;
        private float _radiusSpawn;

        internal event Action<float> AddScoreByAsteroidDead;
        internal event Action<Vector3> IsAsteroidExplosion;
        internal event Action<Vector3, AsteroidType> IsAsteroidExplosionByType;

        public EnemyController(EnemyModel model, SceneController sceneController, PlayerModel playerModel)
        {
            _enemyModel = model;
            _playerModel = playerModel;
            _sceneController = sceneController;
            _sceneController.IsStopControl += OnChangeIsStopControl;
            _activeEnemyList = new();

            _radiusSpawn = _enemyModel.EnemyStruct.RadiusSpawnNewEnemy;
        }

        event Action<float> IEventActionGeneric<float>.OnChangePositionRelativeToAxisY
        {
            add
            {
                AddScoreByAsteroidDead += value;
            }
            remove
            {
                AddScoreByAsteroidDead -= value;
            }
        }



        private void OnChangeIsStopControl(bool value)
        {
            _isStopControl = value;
            if (!_isStopControl)
            {
                GetPoolEnemyAsteroid();
            }
        }

        private void Enemy_IsDead(Asteroid asteroid, bool value)
        {
            if (value)
            {
                if (_activeEnemyList.Contains(asteroid))
                {
                    IsAsteroidExplosion?.Invoke(asteroid.transform.position);
                    IsAsteroidExplosionByType?.Invoke(asteroid.transform.position, asteroid.AsteroidType);
                    _activeEnemyList.Remove(asteroid);
                }

                if (asteroid.AsteroidType == AsteroidType.Meteorite)
                {
                    GetEnemyByTypePool(_enemyModel.EnemyStruct.PoolsOfType[AsteroidType.Cometa], asteroid.transform.localPosition);
                }

                if (_enemyModel.EnemyStruct.PoolsOfType.ContainsKey(asteroid.AsteroidType))
                {
                    AddScoreByAsteroidDead?.Invoke(asteroid.BonusPoints.BonusPointsBeforeDeath);
                    _enemyModel.EnemyStruct.PoolsOfType[asteroid.AsteroidType].ReturnToPool(asteroid);
                }
                else
                {
                    Debug.LogWarning("пулл куда-то потерялся");
                }
            }
        }

        private void GetEnemyByTypePool(AsteroidPool asteroidPool, Vector3 position)
        {
            for (int i = 0; i < 5; i++)
            {
                var enemy = GetAsteroidFromPoolInToPosition(asteroidPool, position);

                enemy.gameObject.SetActive(true);
                enemy.IsDead += Enemy_IsDead;

                var rb = enemy.gameObject.GetOrAddComponent<Rigidbody2D>();
                rb.isKinematic = true;
                rb.velocity = new Vector3(Random.Range(-_radiusSpawn, _radiusSpawn), Random.Range(-_radiusSpawn, _radiusSpawn), 0.0f).normalized * enemy.Speed.CurrentSpeed;

                _activeEnemyList.Add(enemy);
            }
        }

        public void Cleanup()
        {
            foreach (var pool in _enemyModel.EnemyStruct.PoolsOfType.Values)
            {
                foreach (var asteroid in pool.GetList())
                {
                    asteroid.IsDead -= Enemy_IsDead;
                }
            }
        }

        public void Initialization()
        {
            if (!_isStopControl)
            {
                GetPoolEnemyAsteroid();
            }
        }

        private void GetPoolEnemyAsteroid()
        {
            bool newEnemyAdded = false;

            foreach (var poolOfType in _enemyModel.EnemyStruct.PoolsOfType.Values)
            {
                var count = poolOfType.PoolSize;
                if (count <= 0)
                    newEnemyAdded = true;

                for (int i = 0; i < count; i++)
                {
                    GetEnemyFromPool(poolOfType);
                }
            }
            _enemyModel.EnemyStruct.EnemyActivated?.Invoke(newEnemyAdded);
        }

        private Asteroid GetAsteroidFromPoolInToPosition(AsteroidPool pool, Vector3 position)
        {
            var enemy = pool.Get();
            enemy.transform.SetParent(null);
            enemy.transform.position = position;

            return enemy;
        }

        private void GetEnemyFromPool(AsteroidPool pool)
        {
            Vector3 playerPosition = _playerModel.PlayerStruct.Player.transform.position;
            Vector3 randomOffset = new(Random.Range(-_radiusSpawn, _radiusSpawn), Random.Range(-_radiusSpawn, _radiusSpawn), 0.0f);
            Vector3 asteroidPosition = playerPosition + randomOffset;

            var enemy = GetAsteroidFromPoolInToPosition(pool, asteroidPosition);

            enemy.gameObject.SetActive(true);
            enemy.IsDead += Enemy_IsDead;

            var rb = enemy.gameObject.GetOrAddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0.0f) * enemy.Speed.CurrentSpeed;

            _activeEnemyList.Add(enemy);
        }
    }
}