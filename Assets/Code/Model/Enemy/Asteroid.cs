using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Asteroid : EnemyView, IDamageable
    {
        internal Vector2 DirectionMovement;
        internal event Action<Asteroid, bool> IsDead;

        [SerializeField] private float _damage;
        [SerializeField] internal AsteroidType AsteroidType;
        [SerializeField] internal Speed Speed;
        [SerializeField] internal Health Health;

        public float Damage { get => _damage; set => _damage = value; }

        public void TakeDamage(float damage)
        {
            Health.CurrentHealth -= damage;
            if (Health.CurrentHealth <= 0)
            {
                IsDead?.Invoke(this, true);
            }
        }

        public void DealDamage(IDamageable target, float damage)
        {
            Debug.Log($"Deal damage: {damage}");
        }
    }
}