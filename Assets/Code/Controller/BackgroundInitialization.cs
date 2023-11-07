namespace WORLDGAMEDEVELOPMENT
{
    internal class BackgroundInitialization
    {
        private readonly BackgroundFactory _backgroundFactory;
        private readonly BackgroundModel _backgroundModel;

        public BackgroundInitialization(BackgroundFactory backgroundFactory)
        {
            _backgroundFactory = backgroundFactory;
            _backgroundModel = backgroundFactory.CreateModel();
        }

        public BackgroundModel BackgroundModel => _backgroundModel;
    }
}