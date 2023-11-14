using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class VFXController : IController, ICleanup, ILateExecute
    {
        private readonly VFXModel _model;
        private readonly EnemyController _enemyController;
        private readonly PlayerShooterController _playerShooterController;
        private List<DelayExplosionObject> _delayGenericObjects = new List<DelayExplosionObject>();

        public VFXController(VFXModel model, EnemyController enemyController, PlayerShooterController playerShooterController)
        {
            _model = model;
            _enemyController = enemyController;
            _playerShooterController = playerShooterController;

            _enemyController.IsAsteroidExplosionByType += IsAsteroidExplosionByType;
            _playerShooterController.IsCollisionBullet += PlayVFXCollisionDetect;
        }

        private void PlayVFXCollisionDetect(Vector3 position)
        {
            IsAsteroidExplosionByType(position, EnemyType.None);
        }


        public void Cleanup()
        {
            _enemyController.IsAsteroidExplosionByType -= IsAsteroidExplosionByType;
        }

        public void LateExecute(float deltatime)
        {
            for (int i = 0; i < _delayGenericObjects.Count; i++)
            {
                _delayGenericObjects[i].Delay -= deltatime;
                if (_delayGenericObjects[i].Delay < 0)
                {
                    _model.VFXStruct.PoolsVFX[_delayGenericObjects[i].Type].ReturnToPool(_delayGenericObjects[i].Source);
                    _delayGenericObjects.Remove(_delayGenericObjects[i]);
                    i--;
                }
            }
        }

        private void IsAsteroidExplosionByType(Vector3 vector, EnemyType type)
        {
            var explosion = _model.VFXStruct.PoolsVFX[type].Get();
            explosion.gameObject.transform.position = vector;
            explosion.gameObject.SetActive(true);

            if (explosion.main.playOnAwake == false)
                explosion.Play();

            var tempObject = new DelayExplosionObject(explosion, explosion.main.duration, type);

            _delayGenericObjects.Add(tempObject);
        }
    }
}