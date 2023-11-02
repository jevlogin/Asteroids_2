using System.Collections.Generic;
using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal class EnemySettings
    {
        [SerializeField] private float _radiusSpawnNewEnemy;

        public List<EnemySettingsGroup> Enemies = new();

        internal float RadiusSpawnEnemy => _radiusSpawnNewEnemy;
    }
}