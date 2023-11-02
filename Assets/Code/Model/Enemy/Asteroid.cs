using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Asteroid : EnemyView, IDamageable
    {
        #region Fields

        [SerializeField] internal Speed Speed;
        [SerializeField] internal Health Health;
        [SerializeField] private float _damage;
        internal event Action<Asteroid, bool> IsDead;
        internal Vector2 DirectionMovement;
        internal AsteroidType AsteroidType;

        //TODO - убрать сериализованные поля
        [SerializeField] internal BonusPoints BonusPoints;

        #endregion


        #region Properties

        public float Damage { get => _damage; set => _damage = value; }

        #endregion

        private void OnEnable()
        {
            OnCollisionEnterDetect += Asteroid_OnCollisionEnterDetect;
        }

        private void OnDisable()
        {
            OnCollisionEnterDetect -= Asteroid_OnCollisionEnterDetect;
        }

        private void Asteroid_OnCollisionEnterDetect(Collider2D collider)
        {
            if(collider.TryGetComponent<IDamageable>(out var damageable))
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
            }
        }

        #endregion
    }
}