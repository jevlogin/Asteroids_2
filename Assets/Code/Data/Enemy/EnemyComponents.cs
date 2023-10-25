using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal class EnemyComponents
    {
        internal Dictionary<int, Rigidbody2D> _rigidbodiesEnemies = new();
        internal Dictionary<int, Action<Collider2D>> _onCollisionChanges = new();
    }
}