using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class EnemyController : IController, ICleanup
    {
        private EnemyModel _model;
        private const float _radiusSpawn = 20.0f;

        public EnemyController(EnemyModel model)
        {
            _model = model;

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
    }
}