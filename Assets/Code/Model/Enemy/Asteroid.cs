using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Asteroid : EnemyView, IDamageable
    {
        #region Fields

        internal event Action<Asteroid, bool> IsDead;
        internal Speed Speed;
        internal Health Health;
        private Rigidbody2D _rigidbody;
        internal BonusPoints BonusPoints;
        internal Vector2 DirectionMovement;
        internal EnemyType Type;
        internal bool IsDeadSubscribe { get; set; } = false;

        private float _damage;

        #endregion


        #region Properties

        public float Damage { get => _damage; set => _damage = value; }
        internal Rigidbody2D Rigidbody { get => _rigidbody; set => _rigidbody = value; }

        #endregion

        private void Awake()
        {
            if (_rigidbody == null)
                _rigidbody = gameObject.GetOrAddComponent<Rigidbody2D>();
            OnCollisionEnterDetect += Asteroid_OnCollisionEnterDetect;
        }

        private void OnDestroy()
        {
            OnCollisionEnterDetect -= Asteroid_OnCollisionEnterDetect;
        }

        private void Asteroid_OnCollisionEnterDetect(Collider2D collider)
        {
            if (collider.TryGetComponent<IDamageable>(out var damageable))
            {
                TakeDamage(damageable.Damage);
            }
        }

        #region IDamageable

        public void TakeDamage(float damage)
        {
            Health.CurrentHealth -= damage;
            if (Health.CurrentHealth <= 0)
            {
                IsDead?.Invoke(this, true);
                Health.CurrentHealth = Health.MaxHealth;
            }
        }

        #endregion
    }
}