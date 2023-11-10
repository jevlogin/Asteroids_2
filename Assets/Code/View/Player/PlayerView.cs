using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class PlayerView : MonoBehaviour, ICollisionDetect
    {
        #region ICollisionDetect

        public event Action<Collider2D> OnCollisionEnterDetect;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.TryGetComponent<Bullet>(out var bullet))
            {
                OnCollisionEnterDetect?.Invoke(collision.collider);
            }
        }

        #endregion
    }
}