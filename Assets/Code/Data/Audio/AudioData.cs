using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "AudioData/AudioData", order = 51)]
    internal sealed class AudioData : ScriptableObject
    {
        [SerializeField] private AudioStruct _audioStruct;
        [SerializeField] private AudioComponents _audioComponents;
        [SerializeField] private AudioDataSettings _audioSettings;

        internal AudioStruct AudioStruct => _audioStruct;
        internal AudioComponents AudioComponents => _audioComponents;
        internal AudioDataSettings AudioDataSettings => _audioSettings;
    }
}