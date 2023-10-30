namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class SceneModel
    {
        internal SceneStruct SceneStruct;
        internal SceneSettings SceneSettings;

        public SceneModel(SceneStruct sceneStruct, SceneSettings sceneSettings)
        {
            SceneStruct = sceneStruct;
            SceneSettings = sceneSettings;
        }
    }
}