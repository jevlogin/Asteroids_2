using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class Pool<T> where T : Component
    {
        public T Prefab;
        public int Size;

        public Pool(T prefab, int size)
        {
            Prefab = prefab;
            Size = size;
        }
    }
}