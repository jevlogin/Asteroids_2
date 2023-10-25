using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal class EnemyView : MonoBehaviour, ICollisionDetect
    {
        [SerializeField] private Health _health;

        public Health Health
        {
            get => _health;
            protected set => _health = value;
        }

        public event Action<Collider2D> OnCollisionEnterDetect;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEnterDetect?.Invoke(collision.collider);
        }
    }
}