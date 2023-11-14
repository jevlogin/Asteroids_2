using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Bullet : AmmunitionView, IDamageable
    {
        [SerializeField, Range(0, 10)] private float _maxLifetimeOutsideThePool;
        private float _damage = 10.0f;
        private bool _isDead;
        private float lifeTime = 0.0f;

        public float LifeTime
        {
            get => lifeTime;
            set
            {
                lifeTime = value;
                _isDead = false;
            }
        }
        public float MaxLifeTimeOutsideThePool
        {
            get
            {
                return _maxLifetimeOutsideThePool;
            }
        }

        public float Damage { get => _damage; set => _damage = value; }

        private void Awake()
        {
            OnCollisionEnterDetect += Bullet_OnCollisionEnterDetect;
            _isDead = false;
        }

        private void OnDestroy()
        {
            OnCollisionEnterDetect -= Bullet_OnCollisionEnterDetect;
        }
        private void Bullet_OnCollisionEnterDetect(Collider2D collider)
        {
            if (!_isDead)
            {
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    TakeDamage(damageable.Damage);
                } 
            }
        }

        public void TakeDamage(float damage)
        {
            LifeTime += MaxLifeTimeOutsideThePool;
            _isDead = true;
        }
    }
}