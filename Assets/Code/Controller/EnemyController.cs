using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


namespace WORLDGAMEDEVELOPMENT
{
    internal class EnemyController : IController, ICleanup, IEventActionGeneric<float>, IFixedExecute
    {
        private EnemyModel _enemyModel;
        private readonly PlayerModel _playerModel;
        private bool _isStopControl;
        private List<Asteroid> _activeEnemyList;
        private readonly SceneController _sceneController;
        private float _radiusSpawn;

        internal event Action<float> AddScoreByAsteroidDead;


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
                IsAsteroidExplosionByType?.Invoke(asteroid.transform.position, asteroid.AsteroidType);
                AddScoreByAsteroidDead?.Invoke(asteroid.BonusPoints.BonusPointsAfterDeath);

                ReturnToPoolByType(asteroid);

                GetPoolEnemyAsteroid(1);
            }
        }

        private void ReturnToPoolByType(Asteroid asteroid)
        {
            if (_enemyModel.EnemyStruct.PoolsOfType.Count > 0)
            {
                var pool = _enemyModel.EnemyStruct.PoolsOfType.ContainsKey(asteroid.AsteroidType)
                    ? _enemyModel.EnemyStruct.PoolsOfType[asteroid.AsteroidType]
                    : _enemyModel.EnemyStruct.PoolsOfType.FirstOrDefault().Value;

                pool.ReturnToPool(asteroid);
            }
            RemoveAsteroidFromActiveList(asteroid);
        }

        private void RemoveAsteroidFromActiveList(Asteroid asteroid)
        {
            if (_activeEnemyList.Contains(asteroid))
            {
                _activeEnemyList.Remove(asteroid);
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


        private void GetPoolEnemyAsteroid(int countEnemy = 0)
        {
            foreach (var poolOfType in _enemyModel.EnemyStruct.PoolsOfType.Values)
            {
                int count = countEnemy;
                if (count == 0)
                {
                    count = poolOfType.PoolSize;
                }
                else
                {
                    if (poolOfType.PoolSize == 0)
                    {
                        break;
                    }
                }
                for (int i = 0; i < count; i++)
                {
                    GetEnemyFromPool(poolOfType);
                }
            }
        }

        private Asteroid GetAsteroidFromPoolInToPosition(AsteroidPool pool, Vector3 position)
        {
            var enemy = pool.Get();
            enemy.transform.SetParent(null);
            enemy.transform.localPosition = position;

            return enemy;
        }

        private void GetEnemyFromPool(AsteroidPool pool)
        {
            float radiusSpawn = 10.0f;
            float radiusMovement = 6.0f;

            float minAngle = 30f;
            float maxAngle = 150f;
            float minRadians = Mathf.Deg2Rad * minAngle;
            float maxRadians = Mathf.Deg2Rad * maxAngle;

            float radians = Random.Range(minRadians, maxRadians);

            Vector3 position = new Vector3(
                _playerModel.PlayerStruct.Player.transform.localPosition.x + radiusSpawn * Mathf.Cos(radians),
                _playerModel.PlayerStruct.Player.transform.localPosition.y + radiusSpawn * Mathf.Sin(radians),
                0.0f);

            var enemy = GetAsteroidFromPoolInToPosition(pool, position);

            enemy.gameObject.SetActive(true);

            if (!enemy.IsDeadSubscribe)
            {
                enemy.IsDead += Enemy_IsDead;
                enemy.IsDeadSubscribe = true;
            }

            enemy.Rigidbody.isKinematic = true;

            float randomAngle = Random.Range(0, Mathf.PI * 2f);

            Vector3 randomOffset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f) * radiusMovement;
            Vector3 finalPosition = _playerModel.PlayerStruct.Player.transform.position + randomOffset;
            Vector3 directionMovement = (finalPosition - enemy.transform.position).normalized;

            enemy.Rigidbody.velocity = directionMovement * enemy.Speed.CurrentSpeed;

            _activeEnemyList.Add(enemy);
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            float _minSqrDistance = 150.0f;

            for (int i = _activeEnemyList.Count - 1; i >= 0; i--)
            {
                var enemy = _activeEnemyList[i];
                if ((_playerModel.Components.PlayerTransform.position - enemy.transform.position).sqrMagnitude > _minSqrDistance)
                {
                    _activeEnemyList.Remove(enemy);
                    ReturnToPoolByType(enemy);
                    GetPoolEnemyAsteroid(1);
                }
            }
        }

    }
}