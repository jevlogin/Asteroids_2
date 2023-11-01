using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class GroupAudioClip
    {
        public AudioClipType _type;
        public List<AudioClip> AudioClips = new();
    }
}