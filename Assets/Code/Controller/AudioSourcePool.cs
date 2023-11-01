using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class AudioSourcePool : GenericObjectPool<AudioSource>
    {
        public AudioSourcePool(Pool<AudioSource> pool, Transform transform) : base(pool, transform)
        {
        }
    }
}