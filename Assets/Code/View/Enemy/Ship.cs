using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class Ship : EnemyView, IDamageable
    {
        #region Fields


        [SerializeField] internal EnemyType Type;
        [SerializeField] internal EnemyShipType EnemyShipType;

        [SerializeField] internal Speed Speed;
        [SerializeField] internal Health Health;
        internal event Action<Ship, bool> IsDead;
        private Rigidbody2D _rigidbody;

        //TODO - убрать сериализованные поля
        internal int ExpirienceAfterDead;
        [SerializeField] internal BonusPoints BonusPoints;
        [SerializeField] internal Rigidbody2D Rigidbody => _rigidbody;

        public float Damage { get; set; }
        internal bool IsDeadSubscribe { get; set; } = false;

        [SerializeField] internal List<Transform> _barrelPivot;

        #endregion


        private void Awake()
        {
            OnCollisionEnterDetect += Asteroid_OnCollisionEnterDetect;
            _rigidbody = gameObject.GetOrAddComponent<Rigidbody2D>();
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