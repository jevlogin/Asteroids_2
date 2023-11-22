using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{

    [Serializable]
    internal sealed class Player : PlayerView, IDamageable
    {
        #region Fields

        internal Speed Speed;
        internal Health Health;
        internal Shield Shield;
        internal Expirience Expirience;

        internal int Force;

        [SerializeField] private List<GroupObject> _groupObjects;
        internal event Action<float> LifeLeftCount;
        internal event Action EnableShield;

        private float _damage;
        private float _percentDamage = 0.02f;

        internal event Action IsDeadPlayer;
        internal Action<bool> IsCanShoot;


        #endregion


        #region Properties

        public List<GroupObject> GroupObjects { get => _groupObjects; set => _groupObjects = value; }
        public float Damage
        {
            get => _damage;
            set
            {
                _damage = value;
            }
        }

        public float IncreaseDamage()
        {
            var damage = Mathf.CeilToInt(Damage + (Damage * Expirience.CurrentLevel * _percentDamage));
            return damage;
        }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            OnCollisionEnterDetect += Player_OnCollisionEnterDetect;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (GroupObjects[i].Transform == null)
                {
                    GroupObjects[i].Transform = transform.GetChild(i);
                }
            }

            foreach (var groupObject in GroupObjects)
            {
                foreach (var item in groupObject.Transform.GetComponentsInChildren<ParticleSystem>())
                {
                    groupObject.ParticleSystems.Add(item);
                }
            }
        }

        #endregion

        private void OnDestroy()
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
            float shieldDamage = Mathf.Min(damage * Shield.DamageAbsorptionCoefficient, Shield.CurrentValue);
            float healthDamage = damage - shieldDamage;

            Shield.CurrentValue -= shieldDamage;
            Health.CurrentHealth -= healthDamage;

            if (Health.CurrentHealth <= 0)
            {
                Health.CurrentHealth = Health.MaxHealth;
                IsDeadPlayer?.Invoke();
                gameObject.SetActive(false);
                IsCanShoot?.Invoke(false);
            }
            LifeLeftCount?.Invoke(Health.CurrentHealth);
            EnableShield?.Invoke();
        }
    }
}
