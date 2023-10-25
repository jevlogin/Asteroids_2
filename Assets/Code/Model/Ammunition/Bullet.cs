using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Bullet : AmmunitionView, IDamageable
    {
        [SerializeField, Range(0, 10)] private float _maxLifetimeOutsideThePool;
        private float _damage = 10.0f;

        public float LifeTime { get; set; } = 0.0f;

        public float MaxLifeTimeOutsideThePool
        {
            get
            {
                return _maxLifetimeOutsideThePool;
            }
        }

        public float Damage { get => _damage; set => _damage = value; }

        private void OnEnable()
        {
            OnCollisionEnterDetect += Bullet_OnCollisionEnterDetect;
        }

        private void Bullet_OnCollisionEnterDetect(Collider2D collider)
        {
            if (collider.TryGetComponent<IDamageable>(out var damageable))
            {
                TakeDamage(damageable.Damage);
                DealDamage(damageable, Damage);
            }
        }

        public void TakeDamage(float damage)
        {
            LifeTime += MaxLifeTimeOutsideThePool;
        }

        public void DealDamage(IDamageable target, float damage)
        {
            target.TakeDamage(_damage);
        }
    }
}