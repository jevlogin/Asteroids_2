using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class AudioSourceInfo
    {
        public AudioSource Source;
        public float Delay;

        public AudioSourceInfo(AudioSource source, float length)
        {
            Source = source;
            Delay = length;
        }
    }
}