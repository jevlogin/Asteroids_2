using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [RequireComponent(typeof(Camera))]
    internal sealed class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _smoothTime;
        [SerializeField] private float _introSmoothTime;
        private float _defaultSmoothTime;

        private void OnEnable()
        {
            _defaultSmoothTime = _smoothTime;
        }

        public Camera Camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = GetComponent<Camera>();
                }
                return _camera;
            }
        }

        public float SmoothTime
        {
            get
            {
                return _smoothTime;
            }
            set { _smoothTime = value; }
        }

        public float DefaultSmoothTime  => _defaultSmoothTime; 
        public float IntroSmoothTime => _introSmoothTime; 
    }
}