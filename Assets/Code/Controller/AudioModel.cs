namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class AudioModel
    {
        public AudioStruct AudioStruct;
        public AudioComponents Components;
        public AudioDataSettings Settings;

        public AudioModel(AudioStruct audioStruct, AudioComponents components, AudioDataSettings settings)
        {
            this.AudioStruct = audioStruct;
            Components = components;
            Settings = settings;
        }
    }
}