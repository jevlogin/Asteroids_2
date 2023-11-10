using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class RotationController : ICleanup, IFixedExecute
    {
        private Transform _playerTransform;
        private Camera _camera;
        private bool _isStopControl;
        private float _horizontal;
        private float _vertical;
        private readonly SceneController _sceneController;
        private readonly Rigidbody2D _rigidbodyPlayer;

        private float _tiltSpeedForward = 20.0f;
        private float _tiltSpeedBackward = 20.0f;
        private float _tiltSpeedSideways = 10.0f;
        private float _trashHoldRotation = 0.1f;

        public RotationController((IUserInputProxy inputHorizontal, IUserInputProxy inputVertical) input, Transform playerTransform, Camera camera, SceneController sceneController, Rigidbody2D rigidbodyPlayer)
        {
            _playerTransform = playerTransform;
            _camera = camera;
            _sceneController = sceneController;
            _rigidbodyPlayer = rigidbodyPlayer;

            input.inputHorizontal.AxisOnChange += OnChangeInputHorizontal;
            input.inputVertical.AxisOnChange += OnChangeInputVertical;

            _sceneController.IsStopControl += OnChangeIsStopControl;
        }

        private void OnChangeInputVertical(float value)
        {
            _vertical = value;
        }

        private void OnChangeInputHorizontal(float value)
        {
            _horizontal = value;
        }

        private void OnChangeIsStopControl(bool value)
        {
            _isStopControl = value;
        }


        public void FixedExecute(float fixedDeltatime)
        {
            if (_isStopControl) { return; }

            float tiltAngleX = _vertical * (_vertical > 0 ? _tiltSpeedForward : _tiltSpeedBackward);
            float tiltAngleZ = -_horizontal * _tiltSpeedSideways;

            var movement = new Vector3(tiltAngleX, tiltAngleZ, tiltAngleZ);

            if (Mathf.Abs(tiltAngleX) <= _trashHoldRotation && Mathf.Abs(tiltAngleZ) <= _trashHoldRotation)
            {
                var gyroInput = Input.gyro.rotationRateUnbiased;
                tiltAngleX = -gyroInput.x * (gyroInput.x > 0 ? _tiltSpeedForward : _tiltSpeedBackward);
                tiltAngleZ = gyroInput.y * _tiltSpeedSideways * 1.5f;

                if (Mathf.Abs(tiltAngleX) >= _trashHoldRotation && Mathf.Abs(tiltAngleZ) >= _trashHoldRotation)
                {
                    movement = new Vector3(tiltAngleZ, tiltAngleX, tiltAngleX);
                }
            }

            Quaternion targetRotation = Quaternion.Euler(movement);

            _playerTransform.rotation = targetRotation;
        }


        #region ICleanup

        public void Cleanup()
        {
            _sceneController.IsStopControl -= OnChangeIsStopControl;
        }

        

        #endregion
    }
}