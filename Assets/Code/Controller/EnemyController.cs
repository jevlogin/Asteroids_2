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
        private List<Asteroid> _activeAsteroidsList;
        private readonly SceneController _sceneController;
        private float _radiusSpawn;
        private List<Ship> _activeShipsList;

        internal event Action<float> AddScoreByAsteroidDead;


        internal event Action<Vector3, EnemyType> IsAsteroidExplosionByType;

        public EnemyController(EnemyModel model, SceneController sceneController, PlayerModel playerModel)
        {
            _enemyModel = model;
            _playerModel = playerModel;
            _sceneController = sceneController;
            _sceneController.IsStopControl += OnChangeIsStopControl;
            _activeAsteroidsList = new();
            _activeShipsList = new();

            _radiusSpawn = _enemyModel.EnemyStruct.RadiusSpawnNewEnemy;
        }

        //TODO - remove this
        event Action<float> IEventActionGeneric<float>.OnChangePositionAxisY
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
                IsAsteroidExplosionByType?.Invoke(asteroid.transform.position, asteroid.Type);
                AddScoreByAsteroidDead?.Invoke(asteroid.BonusPoints.BonusPointsAfterDeath);
                
                _playerModel.PlayerStruct.Player.Expirience.CurrentValue += asteroid.ExpirienceAfterDead;

                ReturnToPoolByType(asteroid, asteroid.Type);

                GetPoolEnemyAsteroid(1, asteroid.Type);
            }
        }


        private void ReturnToPoolByType(EnemyView enemyView, EnemyType type)
        {
            var pool = _enemyModel.EnemyStruct.PoolsOfType.ContainsKey(type)
                   ? _enemyModel.EnemyStruct.PoolsOfType[type]
                   : _enemyModel.EnemyStruct.PoolsOfType.FirstOrDefault().Value;
            pool.ReturnToPool(enemyView);

            switch (type)
            {
                case EnemyType.Meteorite:
                case EnemyType.Cometa:
                    //RemoveAsteroidFromActiveList(enemyView as Asteroid);
                    _activeAsteroidsList.Remove(enemyView as Asteroid);
                    break;
                case EnemyType.Ship:
                    //RemoveAsteroidFromActiveList(enemyView as Ship);
                    _activeShipsList.Remove(enemyView as Ship);
                    break;
            }
        }
       
        public void Cleanup()
        {
            foreach (var pool in _enemyModel.EnemyStruct.PoolsOfType.Values)
            {
                foreach (var enemy in pool.GetList())
                {
                    if (enemy is Asteroid asteroid)
                    {
                        asteroid.IsDead -= Enemy_IsDead;
                    }
                }
            }
        }

        private void GetPoolEnemyAsteroid(int countEnemy = 0, EnemyType enemyType = EnemyType.None)
        {
            if (countEnemy > 0)
            {
                switch (enemyType)
                {
                    case EnemyType.None:
                        break;
                    case EnemyType.Meteorite:
                    case EnemyType.Cometa:
                        for (int i = 0; i < countEnemy; i++)
                        {
                            GetEnemyFromPool(_enemyModel.EnemyStruct.PoolsOfType[enemyType]);
                        }
                        break;
                    case EnemyType.Ship:
                        for (int i = 0; i < countEnemy; i++)
                        {
                            GetEnemyFromPool(_enemyModel.EnemyStruct.PoolsOfType[enemyType]);
                        }
                        break;
                }
            }
            else
            {
                foreach (var poolOfType in _enemyModel.EnemyStruct.PoolsOfType.Values)
                {
                    if (poolOfType.PoolSize == 0)
                    {
                        break;
                    }
                    int count = poolOfType.PoolSize;
                    for (int i = 0; i < count; i++)
                    {
                        GetEnemyFromPool(poolOfType);
                    }
                }
            }
        }

        private EnemyView GetEnemyViewFromPoolInToPosition(EnemyPool pool, Vector3 position)
        {
            var enemy = pool.Get();
            enemy.transform.SetParent(null);
            enemy.transform.localPosition = position;
            return enemy;
        }

        private void GetEnemyFromPool(EnemyPool pool)
        {
            switch (pool.EnemyGroup.Type)
            {
                case EnemyType.Meteorite:
                case EnemyType.Cometa:
                    float radiusSpawn = 20.0f;
                    float radiusMovement = 10.0f;

                    float minAngle = 10f;
                    float maxAngle = 170f;
                    float minRadians = Mathf.Deg2Rad * minAngle;
                    float maxRadians = Mathf.Deg2Rad * maxAngle;

                    float radians = Random.Range(minRadians, maxRadians);

                    Vector3 position = new Vector3(
                        _playerModel.PlayerStruct.Player.transform.localPosition.x + radiusSpawn * Mathf.Cos(radians),
                        _playerModel.PlayerStruct.Player.transform.localPosition.y + radiusSpawn * Mathf.Sin(radians),
                        0.0f);

                    var enemy = GetEnemyViewFromPoolInToPosition(pool, position) as Asteroid;

                    if (!enemy.IsDeadSubscribe)
                    {
                        enemy.IsDead += Enemy_IsDead;
                        enemy.Health = new Health(pool.EnemyGroup.Health);
                        enemy.Speed = new Speed(pool.EnemyGroup.Speed);
                        enemy.Rigidbody = enemy.gameObject.GetOrAddComponent<Rigidbody2D>();
                        enemy.BonusPoints = new BonusPoints(pool.EnemyGroup.BonusPoints);
                        enemy.ExpirienceAfterDead = pool.EnemyGroup.Expirience;
                        enemy.Type = pool.EnemyGroup.Type;
                        enemy.Damage = pool.EnemyGroup.DefaultDamage;
                        enemy.IsDeadSubscribe = true;
                        enemy.Rigidbody.isKinematic = true;
                    }

                    enemy.gameObject.SetActive(true);

                    float randomAngle = Random.Range(0, Mathf.PI * 2f);
                    Vector3 randomOffset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f) * radiusMovement;
                    Vector3 finalPosition = _playerModel.PlayerStruct.Player.transform.position + randomOffset;
                    Vector3 directionMovement = (finalPosition - enemy.transform.position).normalized;

                    enemy.Rigidbody.velocity = directionMovement * enemy.Speed.CurrentSpeed;

                    _activeAsteroidsList.Add(enemy);
                    break;
                case EnemyType.Ship:
                    float movementRadiusShip = 15.0f; // Радиус движения корабля

                    float shipSpawnAngle = Random.Range(0, Mathf.PI); // Только сверху (от 0 до π)
                    Vector3 shipSpawnOffset = new Vector3(Mathf.Cos(shipSpawnAngle), Mathf.Sin(shipSpawnAngle), 0f) * _radiusSpawn;
                    Vector3 shipSpawnPosition = new Vector3(
                        _playerModel.PlayerStruct.Player.transform.position.x + shipSpawnOffset.x,
                        _playerModel.PlayerStruct.Player.transform.position.y + _radiusSpawn + shipSpawnOffset.y,
                        0f);

                    var enemyShip = GetEnemyViewFromPoolInToPosition(pool, shipSpawnPosition) as Ship;

                    if (!enemyShip.IsDeadSubscribe)
                    {
                        enemyShip.IsDead += EnemyShip_IsDead;

                        enemyShip.Health = new Health(pool.EnemyGroup.Health);
                        enemyShip.Speed = new Speed(pool.EnemyGroup.Speed);
                        enemyShip.BonusPoints = new BonusPoints(pool.EnemyGroup.BonusPoints);
                        enemyShip.ExpirienceAfterDead = pool.EnemyGroup.Expirience;
                        enemyShip.Type = pool.EnemyGroup.Type;
                        enemyShip.Damage = pool.EnemyGroup.DefaultDamage;

                        enemyShip.IsDeadSubscribe = true;
                        enemyShip.Rigidbody.isKinematic = true;
                    }

                    enemyShip.gameObject.SetActive(true);

                    float shipRandomAngle = Random.Range(0, Mathf.PI * 2f);
                    Vector3 shipRandomOffset = new Vector3(Mathf.Cos(shipRandomAngle), Mathf.Sin(shipRandomAngle), 0f) * movementRadiusShip;
                    Vector3 shipFinalPosition = _playerModel.PlayerStruct.Player.transform.position + shipRandomOffset;
                    Vector3 shipDirectionMovement = (shipFinalPosition - enemyShip.transform.position).normalized;

                    enemyShip.Rigidbody.velocity = shipDirectionMovement * enemyShip.Speed.CurrentSpeed;

                    _activeShipsList.Add(enemyShip);
                    break;
                default:
                    break;
            }

        }

        private void EnemyShip_IsDead(Ship ship, bool value)
        {
            IsAsteroidExplosionByType?.Invoke(ship.transform.position, ship.Type);
            AddScoreByAsteroidDead?.Invoke(ship.BonusPoints.BonusPointsAfterDeath);

            _playerModel.PlayerStruct.Player.Expirience.CurrentValue += ship.ExpirienceAfterDead;

            ReturnToPoolByType(ship, ship.Type);

            GetPoolEnemyAsteroid(1, ship.Type);
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            float _minSqrDistance = 30.0f;

            for (int i = _activeAsteroidsList.Count - 1; i >= 0; i--)
            {
                var enemy = _activeAsteroidsList[i];
                var distance = Vector3.Distance(_playerModel.Components.PlayerTransform.position, enemy.transform.position);
                if (distance > _minSqrDistance)
                {
                    ReturnToPoolByType(enemy, enemy.Type);
                    GetPoolEnemyAsteroid(1, enemy.Type);
                }
            }

            for (int i = _activeShipsList.Count - 1; i >= 0; i--)
            {
                var ship = _activeShipsList[i];
                var distance = Vector3.Distance(_playerModel.Components.PlayerTransform.position, ship.transform.position);
                if (distance > _minSqrDistance)
                {
                    ReturnToPoolByType(ship, ship.Type);
                    GetPoolEnemyAsteroid(1, ship.Type);
                }
            }

        }
    }
}