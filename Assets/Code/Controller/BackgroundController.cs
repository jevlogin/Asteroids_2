using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class BackgroundController : IController, IInitialization, ICleanup, IFixedExecute
    {
        private bool _isStopControl;
        private readonly SceneController _sceneController;
        private readonly BackgroundModel _backgroundModel;
        private readonly Camera _camera;

        private List<VFXPool> _backgroundPools = new List<VFXPool>();

        private List<ParticleSystem> _activeBackgroundObjects;
        private List<BackgroundView> _activeBackgroundViews;

        Dictionary<GameObject, Vector3> objectPositions = new Dictionary<GameObject, Vector3>();

        private float minSpacingSqr;
        private float minSpacing = 5.0f;

        private float spacing = 25.0f;

        private float minZ = -5.0f;
        private float maxZ = 5.0f;
        private float _speedMovement = 2.0f;
        private List<PoolBackground> _backgroundPoolsBack;

        public BackgroundController(BackgroundModel backgroundModel, Camera camera, SceneController sceneController)
        {
            _sceneController = sceneController;
            _backgroundModel = backgroundModel;
            _camera = camera;
            minSpacingSqr = Mathf.Pow(minSpacing, 2);
        }


        public void Initialization()
        {
            _activeBackgroundObjects = new();
            _activeBackgroundViews = new();
            _backgroundPoolsBack = new();
            _sceneController.IsStopControl += OnChangeStopControl;
            _isStopControl = true;

            foreach (var pool in _backgroundModel.Structure.PoolsType.Values)
            {
                foreach (var item in pool)
                {
                    if (item is VFXPool vFX)
                    {
                        _backgroundPools.Add(vFX);
                    }
                    if (item is PoolBackground background)
                    {
                        _backgroundPoolsBack.Add(background);
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

            //for new objects backgroundPool 
            foreach (var backgroundPool in _backgroundPoolsBack)
            {
                var backgroundObject = backgroundPool.Get();
                Vector3 newPosition = Vector3.zero;

                newPosition = GetRandomPositionAboveCamera();

                foreach (var kvp in objectPositions)
                {
                    var positionObject = kvp.Value;

                    if ((newPosition - positionObject).sqrMagnitude < minSpacingSqr)
                    {
                        newPosition += kvp.Value * spacing;
                        break;
                    }
                }

                objectPositions.Add(backgroundObject.gameObject, newPosition);

                backgroundObject.transform.position = newPosition;
                backgroundObject.gameObject.SetActive(true);

                _activeBackgroundViews.Add(backgroundObject);
            }
            objectPositions.Clear();
        }

        public void FixedExecute(float fixedDeltatime)
        {
            if (!_isStopControl && _activeBackgroundObjects.Count > 0)
            {
                float minY = _camera.transform.position.y - _camera.orthographicSize - spacing;
                var newPosition = _speedMovement * fixedDeltatime * Vector3.down;

                for (int i = _activeBackgroundObjects.Count - 1; i >= 0; i--)
                {
                    var backgroundObject = _activeBackgroundObjects[i];
                    backgroundObject.transform.position += newPosition;

                    if (backgroundObject.transform.position.y < minY)
                    {
                        _activeBackgroundObjects.Remove(backgroundObject);
                        backgroundObject.gameObject.SetActive(false);
                        _backgroundPools.FirstOrDefault()?.ReturnToPool(backgroundObject);  //TODO - различать пулы
                    }
                }

                for (int i = _activeBackgroundViews.Count - 1; i >= 0; i--)
                {
                    var backgroundObject = _activeBackgroundViews[i];
                    backgroundObject.transform.position += newPosition;

                    if (backgroundObject.transform.position.y < minY)
                    {
                        _activeBackgroundViews.Remove(backgroundObject);
                        backgroundObject.gameObject.SetActive(false);
                        _backgroundPoolsBack.FirstOrDefault()?.ReturnToPool(backgroundObject);
                    }
                }
            }

            if ((_activeBackgroundViews.Count <= 0 || _activeBackgroundObjects.Count <= 0) && !_isStopControl)
            {
                SpawnBackground();
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