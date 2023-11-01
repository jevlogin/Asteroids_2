using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class AudioFactory
    {
        private readonly AudioData _audioData;
        private AudioModel _audioModel;

        public AudioFactory(AudioData audioData)
        {
            _audioData = audioData;
        }

        internal AudioModel CreateModel()
        {
            var audioStruct = _audioData.AudioStruct;
            var components = new AudioComponents();
            var settings = new AudioDataSettings();

            var audioSourceEffects = Object.Instantiate(_audioData.AudioDataSettings.AudioSourceByMixerTypesPrefabs.FirstOrDefault(source => source.mixerGroup == MixerGroupByName.Effects).AudioSource);
            var poolAudioSourceEffect = new Pool<AudioSource>(audioSourceEffects, _audioData.AudioDataSettings.PoolSize);

            var poolAudioSourceTransfromParent = new GameObject(ManagerName.POOL_AUDIO);
            var poolAudioEffectsGeneric = new AudioSourcePool(poolAudioSourceEffect, poolAudioSourceTransfromParent.transform);
            poolAudioEffectsGeneric.AddObjects(audioSourceEffects);

            audioStruct.PoolsByMixerTypes = new Dictionary<MixerGroupByName, AudioSourcePool>
            {
                [MixerGroupByName.Effects] = poolAudioEffectsGeneric
            };

            _audioModel = new AudioModel(audioStruct, components, settings);
            return _audioModel;
        }
    }
}