using System;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public class EnemySettingsGroup
    {
        [SerializeField] internal AsteroidType Type;
        [SerializeField] internal Sprite Sprite;
        [SerializeField] internal float DefaultDamage;
        [SerializeField] internal Speed Speed;
        [SerializeField] internal Health Health;
        [SerializeField] internal BonusPoints BonusPoints;
        [SerializeField] internal int PoolSize;
    }
}