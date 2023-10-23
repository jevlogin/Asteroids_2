using UnityEngine;
using UnityEngine.UIElements;

namespace WORLDGAMEDEVELOPMENT
{
    internal class RotationController : IExecute
    {
        private Transform _playerTransform;
        private Camera _camera;

        public RotationController(Transform playerTransform, Camera camera)
        {
            _playerTransform = playerTransform;
            _camera = camera;
        }

        public void Execute(float deltatime)
        {
            var direction = Input.mousePosition - _camera.WorldToScreenPoint(_playerTransform.position);

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            _playerTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}