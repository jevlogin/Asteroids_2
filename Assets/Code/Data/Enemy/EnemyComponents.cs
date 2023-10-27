using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal class EnemyComponents
    {
        internal Dictionary<int, Rigidbody2D> RigidbodiesEnemies = new();
        internal Dictionary<int, Collider2D> ColliderEnemies = new();
        internal List<EnemyView> ListEnemyViews = new();
    }
}