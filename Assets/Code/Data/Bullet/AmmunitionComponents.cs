using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class AmmunitionComponents
    {
        [SerializeField] private AmmunitionView _ammunitionView;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private CircleCollider2D _circleCollider2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public AmmunitionView AmmunitionView => _ammunitionView;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public CircleCollider2D CircleCollider2D => _circleCollider2D;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
    }
}