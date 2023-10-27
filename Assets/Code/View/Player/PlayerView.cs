using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class PlayerView : MonoBehaviour, ICollisionDetect
    {
        public event Action<Collider2D> OnCollisionEnterDetect = delegate(Collider2D collider2D) { };

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEnterDetect.Invoke(collision.collider);
        }
    }
}