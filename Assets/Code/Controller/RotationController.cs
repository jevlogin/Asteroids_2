using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class RotationController : IExecute, ICleanup
    {
        private Transform _playerTransform;
        private Camera _camera;
        private bool _isStopControl;
        private readonly SceneController _sceneController;
        private readonly Rigidbody2D _rigidbodyPlayer;

        public RotationController(Transform playerTransform, Camera camera, SceneController sceneController, Rigidbody2D rigidbodyPlayer)
        {
            _playerTransform = playerTransform;
            _camera = camera;
            _sceneController = sceneController;
            _rigidbodyPlayer = rigidbodyPlayer;

            _sceneController.IsStopControl += OnChangeIsStopControl;
        }

        private void OnChangeIsStopControl(bool value)
        {
            _isStopControl = value;
        }

        public void Execute(float deltatime)
        {
            if (_isStopControl) { return; }

            var direction = Input.mousePosition - _camera.WorldToScreenPoint(_playerTransform.position);

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;

            //_playerTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            _rigidbodyPlayer.SetRotation(angle);
        }


        #region ICleanup

        public void Cleanup()
        {
            _sceneController.IsStopControl -= OnChangeIsStopControl;
        }

        #endregion
    }
}