using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Player : PlayerView, IDamageable
    {
        [SerializeField] internal Speed Speed;
        [SerializeField] internal Health Health;
        internal int Force;
        private float _damage;

        public float Damage { get => _damage; set => _damage = value; }

        private void OnEnable()
        {
            OnCollisionEnterDetect += Player_OnCollisionEnterDetect;
        }

        private void OnDisable()
        {
            OnCollisionEnterDetect -= Player_OnCollisionEnterDetect;
        }

        private void Player_OnCollisionEnterDetect(Collider2D collider)
        {
            if (collider.TryGetComponent<IDamageable>(out var damageable))
            {
                TakeDamage(damageable.Damage);
            }
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("Игрок столкнулся с кем-то");
            Health.CurrentHealth -= damage;
            if (Health.CurrentHealth <= 0)
            {
                Debug.Log("I'm dead");
                Health.CurrentHealth = Health.MaxHealth;
            }
        }
    }
}
