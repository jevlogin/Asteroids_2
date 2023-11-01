namespace WORLDGAMEDEVELOPMENT
{
    internal class AudioInitialization
    {
        private readonly AudioFactory _audioFactory;
        private readonly AudioModel _audioModel;

        internal AudioModel AudioModel => _audioModel;

        public AudioInitialization(AudioFactory audioFactory)
        {
            _audioFactory = audioFactory;
            _audioModel = _audioFactory.CreateModel();
        }
    }
}