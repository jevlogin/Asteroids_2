﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class BackgroundController : IController, IInitialization, ICleanup, IFixedExecute
    {
        private PlayerController _playerController;
        private EnemyController _enemyController;
        private bool _isStopControl;
        private readonly SceneController _sceneController;
        private readonly BackgroundModel _backgroundModel;
        private readonly Camera _camera;

        private List<VFXPool> _backgroundPools = new List<VFXPool>();

        private List<ParticleSystem> _activeBackgroundObjects;

        Dictionary<GameObject, Vector3> objectPositions = new Dictionary<GameObject, Vector3>();
        private float minSpacingSqr = 30.0f;
        private float spacing = 25.0f;
        private float minZ = -5.0f;
        private float maxZ = 5.0f;
        private float _speedMovement = 2.0f;

        public BackgroundController(BackgroundModel backgroundModel, Camera camera, PlayerController playerController, EnemyController enemyController, SceneController sceneController)
        {
            _playerController = playerController;
            _enemyController = enemyController;
            _sceneController = sceneController;
            _backgroundModel = backgroundModel;
            _camera = camera;
        }

        public void Initialization()
        {
            _activeBackgroundObjects = new();
            _sceneController.IsStopControl += OnChangeStopControl;

            foreach (var pool in _backgroundModel.Structure.PoolsType.Values)
            {
                foreach (var item in pool)
                {
                    if (item is VFXPool vFX)
                    {
                        _backgroundPools.Add(vFX);
                    }
                }
            }
        }

        private Vector3 GetRandomPositionAboveCamera()
        {
            float cameraY = _camera.transform.position.y;
            float y = cameraY + _camera.orthographicSize + spacing;
            float x = Random.Range(-_camera.orthographicSize * _camera.aspect, _camera.orthographicSize * _camera.aspect);
            float z = Random.Range(minZ, maxZ);
            return new Vector3(x, y, z);
        }


        private void OnChangeStopControl(bool value)
        {
            _isStopControl = value;
            //Запуск противников
            
            if (!_isStopControl)
            {
                SpawnBackground();
            }
        }

        private void SpawnBackground()
        {
            foreach (var pool in _backgroundPools)
            {
                var newBackgroundObject = pool.Get();

                Vector3 newPosition = Vector3.zero;

                newPosition = GetRandomPositionAboveCamera();

                foreach (var kvp in objectPositions)
                {
                    if ((newPosition - kvp.Value).sqrMagnitude < minSpacingSqr)
                    {
                        newPosition += kvp.Value * spacing;
                        break;
                    }
                }

                objectPositions.Add(newBackgroundObject.gameObject, newPosition);
                newBackgroundObject.transform.position = newPosition;
                newBackgroundObject.gameObject.SetActive(true);

                var rotation = GetRandomPositionAboveCamera();
                newBackgroundObject.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

                _activeBackgroundObjects.Add(newBackgroundObject);
            }
            objectPositions.Clear();
        }

        public void FixedExecute(float fixedDeltatime)
        {
            if (!_isStopControl && _activeBackgroundObjects.Count > 0)
            {
                float minY = _camera.transform.position.y - _camera.orthographicSize - spacing;

                for (int i = _activeBackgroundObjects.Count - 1; i >= 0; i--)
                {
                    var backgroundObject = _activeBackgroundObjects[i];
                    backgroundObject.transform.position += Vector3.down * _speedMovement * fixedDeltatime;

                    if (backgroundObject.transform.position.y < minY)
                    {
                        _activeBackgroundObjects.RemoveAt(i);
                        backgroundObject.gameObject.SetActive(false);

                        //TODO - научиться распознавать пуллы
                        _backgroundPools.FirstOrDefault().ReturnToPool(backgroundObject);
                        SpawnBackground();
                    }
                }
            }
        }


        #region ICleanup

        public void Cleanup()
        {
            _sceneController.IsStopControl -= OnChangeStopControl;
        }

        #endregion
    }
}