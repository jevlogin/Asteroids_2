using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "SceneData/SceneData", order = 51)]
    internal sealed class SceneData : ScriptableObject
    {
        [SerializeField] private SceneStruct _sceneStruct;
        [SerializeField] private SceneSettings _sceneSettings;

        internal SceneStruct SceneStruct => _sceneStruct;
        internal SceneSettings SceneSettings => _sceneSettings;
    }
}