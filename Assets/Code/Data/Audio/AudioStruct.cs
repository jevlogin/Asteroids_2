using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct AudioStruct
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private List<GroupAudioClip> _clipByTypes;


        internal AudioSourcePool AudioSourcePoolEffects
        {
            get
            {
                return PoolsByMixerTypes[MixerGroupByName.Effects];
            }
        }

        internal Dictionary<MixerGroupByName, AudioSourcePool> PoolsByMixerTypes;

        //internal AudioSourcePool AudioSourcePoolUI;
        //internal AudioSourcePool AudioSourcePoolMusic;


        internal AudioMixer AudioMixer => _audioMixer;
        internal List<GroupAudioClip> ClipByTypes { get => _clipByTypes; }
    }
}