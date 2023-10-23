using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [RequireComponent(typeof(Camera))]
    internal sealed class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _smoothTime;

        public Camera Camera => _camera;
        public float SmoothTime => _smoothTime;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }
    }
}