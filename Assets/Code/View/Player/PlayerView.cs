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
            OnCollisionEnterDetect?.Invoke(collision.collider);
        }

        #endregion
    }
}