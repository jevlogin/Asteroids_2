using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public class EnemySettingsGroup
    {
        public AsteroidType Type;
        public Sprite sprite;
        public float defaultDamage = 10.0f;
        public float speed = 5.0f;
    }
}