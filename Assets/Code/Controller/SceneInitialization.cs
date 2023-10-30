namespace WORLDGAMEDEVELOPMENT
{
    internal class SceneInitialization
    {
        private readonly SceneFactory _sceneFactory;
        private SceneModel _sceneModel;

        internal SceneModel SceneModel
        {
            get
            {
                if (_sceneModel == null)
                {
                    _sceneModel = _sceneFactory.CreateScenModel();
                }
                return _sceneModel;
            }
        }

        public SceneInitialization(SceneFactory sceneFactory)
        {
            _sceneFactory = sceneFactory;
        }
    }
}