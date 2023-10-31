using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class EnemyController : IController, ICleanup, IInitialization
    {
        private EnemyModel _model;
        private bool _isStopControl;
        private readonly SceneController _sceneController;
        private const float _radiusSpawn = 100.0f;

        public EnemyController(EnemyModel model, SceneController sceneController)
        {
            _model = model;
            _sceneController = sceneController;
            _sceneController.IsStopControl += OnChangeIsStopControl;
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
                if (_model.EnemyStruct.PoolsOfType.ContainsKey(asteroid.AsteroidType))
                {
                    _model.EnemyStruct.PoolsOfType[asteroid.AsteroidType].ReturnToPool(asteroid);
                }
                else
                {
                    Debug.LogWarning("пулл куда-то потерялся");
                }
            }
        }

        public void Cleanup()
        {
            foreach (var pool in _model.EnemyStruct.PoolsOfType.Values)
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
            foreach (var poolOfType in _model.EnemyStruct.PoolsOfType.Values)
            {
                var count = poolOfType.PoolSize;

                for (int i = 0; i < count; i++)
                {
                    var enemy = poolOfType.Get();
                    enemy.transform.SetParent(null);
                    enemy.transform.position = new Vector3(Random.Range(-_radiusSpawn, _radiusSpawn), Random.Range(-_radiusSpawn, _radiusSpawn), 0.0f);
                    enemy.gameObject.SetActive(true);
                    enemy.IsDead += Enemy_IsDead;

                    var rb = enemy.gameObject.GetOrAddComponent<Rigidbody2D>();
                    rb.isKinematic = true;
                    rb.velocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0.0f) * enemy.Speed.CurrentSpeed;
                }
            }
        }
    }
}