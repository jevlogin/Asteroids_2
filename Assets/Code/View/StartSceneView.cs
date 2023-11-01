using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class StartSceneView : MonoBehaviour
    {
        [SerializeField] private Transform _startSpaceTransform;
        [SerializeField] private GameObject _sceneBackground;

        public Transform StartSpaceTransform { get => _startSpaceTransform; }
        public GameObject SceneBackground { get => _sceneBackground; }
    }
}