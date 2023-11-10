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
        [SerializeField, Range(0.5f, 0.9f)] internal float MinRandPitch;
        [SerializeField, Range(1f, 1.5f)] internal float MaxRandPitch;

        //internal AudioSourcePool AudioSourcePoolUI;
        //internal AudioSourcePool AudioSourcePoolMusic;

        internal AudioMixer AudioMixer => _audioMixer;
        internal List<GroupAudioClip> ClipByTypes { get => _clipByTypes; }
    }
}