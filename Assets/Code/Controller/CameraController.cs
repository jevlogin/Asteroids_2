using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class CameraController : ILateExecute
    {
        private readonly Camera _camera;
        private readonly Transform _playerTransform;
        private CameraView _cameraView;
        private Vector3 velocity = Vector3.zero;

        public CameraController(CameraView cameraView, Transform playerTransform)
        {
            _cameraView = cameraView;
            _camera = _cameraView.Camera;
            _playerTransform = playerTransform;
        }

        public void LateExecute(float deltatime)
        {
            if (_playerTransform == null)
                return;

            Vector3 targetPosition = new Vector3(_playerTransform.position.x, _playerTransform.position.y, _camera.transform.position.z);
            _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, targetPosition, ref velocity, _cameraView.SmoothTime);
        }
    }
}