using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class SceneFactory
    {
        private readonly SceneData _sceneData;
        private SceneModel _sceneModel;

        public SceneFactory(SceneData sceneData)
        {
            _sceneData = sceneData;
        }

        internal SceneModel CreateScenModel()
        {
            if (_sceneModel == null)
            {
                var sceneStruct = _sceneData.SceneStruct;
                var sceneSettings = _sceneData.SceneSettings;

                sceneStruct.StartSceneView = Object.Instantiate(sceneSettings.StartSceneView);
                sceneStruct.StartSceneView.name = sceneSettings.StartSceneView.name;

                sceneStruct.BroadcastEventManager = Object.Instantiate(sceneSettings.BroadcastEventManager);
                sceneStruct.BroadcastEventManager.name = sceneSettings.BroadcastEventManager.name;

                _sceneModel = new SceneModel(sceneStruct, sceneSettings);
            }

            return _sceneModel;
        }
    }
}