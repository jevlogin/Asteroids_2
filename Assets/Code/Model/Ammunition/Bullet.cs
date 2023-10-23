using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Bullet : AmmunitionView
    {
        [SerializeField, Range(0, 10)] private float _timeDestroy;

        public float TimeDestroy { get => _timeDestroy; private set => _timeDestroy = value; }
    }
}