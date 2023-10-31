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
        internal int Force;
        
        [SerializeField] private List<GroupObject> _groupObjects;
        internal event Action<float> TakeDamaged;

        private float _damage;
        internal event Action IsDeadPlayer;


        #endregion


        #region Properties

        public List<GroupObject> GroupObjects { get => _groupObjects; set => _groupObjects = value; }
        public float Damage { get => _damage; set => _damage = value; }

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
            Health.CurrentHealth -= damage;
            TakeDamaged?.Invoke(Health.CurrentHealth);

            if (Health.CurrentHealth <= 0)
            {
                IsDeadPlayer?.Invoke();
                gameObject.SetActive(false);
                Health.CurrentHealth = Health.MaxHealth;
            }
        }
    }
}
