using System;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public class DelayGenericObject<T> where T : Component
    {
        public T Source;
        public float Delay;

        public DelayGenericObject(T source, float length)
        {
            Source = source;
            Delay = length;
        }
    }
}