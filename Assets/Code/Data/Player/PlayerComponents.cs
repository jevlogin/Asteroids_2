using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class PlayerComponents
    {
        public SpriteRenderer SpriteRenderer;
        public CircleCollider2D CircleCollider2D;
        public Transform PlayerTransform;
        public Transform BarrelTransform;
        public Rigidbody2D BulletRigidbody;
        public PlayerView PlayerView;
    }
}