using System;
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
        private bool _cameraViewNormilize;
        private bool _isParticleStarted;

        public CameraController(CameraView cameraView, Transform playerTransform, SceneController sceneController)
        {
            _cameraView = cameraView;
            _camera = _cameraView.Camera;
            _playerTransform = playerTransform;

            _sceneController = sceneController;
            _sceneController.IsStopControl += SceneControllerOnStopControl;
            _sceneController.TakeOffOfTheShip += OnChangeTakeOffOfTheShip;

            _cameraViewNormilize = false;
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
            else if (!_isParticleStarted && _cameraView.SmoothTime <= _cameraView.DefaultSmoothTime)
            {
                _isParticleStarted = true;
            }
            if (!_cameraViewNormilize)
            {
                if (_cameraView.SmoothTime == _cameraView.DefaultSmoothTime)
                {
                    _cameraViewNormilize = true;
                } 
            }


            Vector3 targetPosition = new(_playerTransform.position.x, _playerTransform.position.y, _camera.transform.position.z);
            _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, targetPosition, ref velocity, _cameraView.SmoothTime);
        }


        public void Cleanup()
        {
            _sceneController.IsStopControl -= SceneControllerOnStopControl;
            _sceneController.TakeOffOfTheShip -= OnChangeTakeOffOfTheShip;
        }
    }
}