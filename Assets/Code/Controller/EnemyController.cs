using System.Linq;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class EnemyController : IController, ICleanup
    {
        private EnemyModel _model;

        public EnemyController(EnemyModel model)
        {
            _model = model;

            var count = _model.EnemyStruct.PoolAsteroids.PoolSize;
            for (int i = 0; i < count; i++)
            {
                var enemy = _model.EnemyStruct.PoolAsteroids.Get();
                enemy.transform.SetParent(null);
                enemy.transform.position = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0.0f);
                enemy.gameObject.SetActive(true);
                enemy.IsDead += Enemy_IsDead;

                var rb = enemy.gameObject.GetOrAddComponent<Rigidbody2D>();
                rb.isKinematic = true;
                rb.velocity = Vector3.one * 2;
            }
        }

        private void Enemy_IsDead(Asteroid asteroid, bool value)
        {
            if (value)
            {
                _model.EnemyStruct.PoolAsteroidsList.FirstOrDefault().ReturnToPool(asteroid);
            }
        }

        public void Cleanup()
        {
            foreach (var pool in _model.EnemyStruct.PoolAsteroidsList)
            {
                foreach (var asteroid in pool.GetList())
                {
                    asteroid.IsDead -= Enemy_IsDead;
                }
            }
        }
    }
}