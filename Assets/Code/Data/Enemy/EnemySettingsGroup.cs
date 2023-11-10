using System;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public class EnemySettingsGroup
    {
        [SerializeField] internal EnemyType Type;
        [SerializeField] internal EnemyView PrefabEnemy;
        [SerializeField] internal float DefaultDamage;
        [SerializeField] internal Speed Speed;
        [SerializeField] internal Health Health;
        [SerializeField] internal BonusPoints BonusPoints;
        [SerializeField] internal int PoolSize;
    }
}