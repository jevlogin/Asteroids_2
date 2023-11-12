using System;
using TMPro;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class CameraController : ILateExecute, IAddedModel, ICleanup
    {
        private readonly Camera _camera;
        private readonly Transform _playerTransform;
        private readonly SceneController _sceneController;
        private CameraView _cameraView;
        private Vector3 velocity = Vector3.zero;
        private bool _stopControl;
        private Vector3 _offsetPosition;
        private bool _cameraAligned;

        public CameraController(CameraView cameraView, Transform playerTransform, SceneController sceneController)
        {
            _cameraView = cameraView;
            _camera = _cameraView.Camera;
            _playerTransform = playerTransform;
            _offsetPosition = new Vector3(_camera.transform.position.x, _camera.orthographicSize * 1.6f, _camera.transform.position.z);
            _sceneController = sceneController;
            _sceneController.IsStopControl += SceneControllerOnStopControl;
            _sceneController.TakeOffOfTheShip += OnChangeTakeOffOfTheShip;
        }

        private void OnChangeTakeOffOfTheShip(bool value)
        {
            _stopControl = false;
            _cameraView.SmoothTime = _cameraView.IntroSmoothTime;
        }

        private void SceneControllerOnStopControl(bool value)
        {
            _stopControl = value;
            _cameraView.SmoothTime = _cameraView.DefaultSmoothTime;
        }

        public void LateExecute(float deltatime)
        {
            if (_playerTransform == null || _stopControl)
                return;

            if (_cameraView.SmoothTime > _cameraView.DefaultSmoothTime)
            {
                var time = _cameraView.DefaultSmoothTime / _cameraView.SmoothTime;
                _cameraView.SmoothTime -= time * deltatime;
            }

            if (Vector3.Distance(_camera.transform.position, _playerTransform.position + _offsetPosition) > 0.1f)
            {
                _cameraAligned = false;
            }

            if (!_cameraAligned)
            {
                Vector3 targetPosition = _playerTransform.position + _offsetPosition;
                _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, targetPosition, ref velocity, _cameraView.SmoothTime);

                if (Vector3.Distance(_camera.transform.position, targetPosition) < 0.01f)
                {
                    _cameraAligned = true;
                }
            }
        }


        public void Cleanup()
        {
            _sceneController.IsStopControl -= SceneControllerOnStopControl;
            _sceneController.TakeOffOfTheShip -= OnChangeTakeOffOfTheShip;
        }
    }
}