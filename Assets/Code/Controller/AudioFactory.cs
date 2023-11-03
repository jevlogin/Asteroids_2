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

            var rootTransfromParent = new GameObject(ManagerName.POOL_AUDIO);

            var audioSourceEffects = Object.Instantiate(_audioData.AudioDataSettings.AudioSourceByMixerTypesPrefabs.FirstOrDefault(source => source.mixerGroup == MixerGroupByName.Effects).AudioSource);
            var poolAudioSourceEffect = new Pool<AudioSource>(audioSourceEffects, _audioData.AudioDataSettings.PoolSize);

            var poolAudioEffectsGeneric = new AudioSourcePool(poolAudioSourceEffect, rootTransfromParent.transform);
            poolAudioEffectsGeneric.AddObjects(audioSourceEffects);

            var audioSourceMusic = Object.Instantiate(_audioData.AudioDataSettings.AudioSourceByMixerTypesPrefabs.FirstOrDefault(source => source.mixerGroup == MixerGroupByName.Music).AudioSource);
            var poolSourceMusic = new Pool<AudioSource>(audioSourceMusic, _audioData.AudioDataSettings.PoolSize);

            var poolAudioMusicGeneric = new AudioSourcePool(poolSourceMusic, rootTransfromParent.transform);
            poolAudioMusicGeneric.AddObjects(audioSourceMusic);
           

            audioStruct.PoolsByMixerTypes = new Dictionary<MixerGroupByName, AudioSourcePool>
            {
                [MixerGroupByName.Effects] = poolAudioEffectsGeneric,
                [MixerGroupByName.Music] = poolAudioMusicGeneric,
            };


            _audioModel = new AudioModel(audioStruct, components, settings);
            return _audioModel;
        }
    }
}