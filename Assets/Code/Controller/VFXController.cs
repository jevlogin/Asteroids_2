using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class VFXController : IController, ICleanup, ILateExecute
    {
        private readonly VFXModel _model;
        private readonly EnemyController _enemyController;

        private List<DelayExplosionObject> _delayGenericObjects = new List<DelayExplosionObject>();

        public VFXController(VFXModel model, EnemyController enemyController)
        {
            _model = model;
            _enemyController = enemyController;

            _enemyController.IsAsteroidExplosionByType += IsAsteroidExplosionByType;
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

        private void IsAsteroidExplosionByType(Vector3 vector, AsteroidType type)
        {
            var explosion = _model.VFXStruct.PoolsVFX[type].Get();
            explosion.gameObject.transform.position = vector;
            explosion.gameObject.SetActive(true);

            var tempObject = new DelayExplosionObject(explosion, explosion.main.duration, type);

            _delayGenericObjects.Add(tempObject);
        }
    }
}