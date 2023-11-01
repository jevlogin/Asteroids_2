using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class AudioDataSettings
    {
        [SerializeField] private List<AudioSourceByMixerGroupType> _audioSourceByGroupTypesPrefabs;
        [SerializeField] private int _poolSize;

        public int PoolSize => _poolSize;
        internal List<AudioSourceByMixerGroupType> AudioSourceByMixerTypesPrefabs => _audioSourceByGroupTypesPrefabs;
    }
}