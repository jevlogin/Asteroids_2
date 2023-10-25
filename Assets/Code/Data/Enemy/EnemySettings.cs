using System.Collections.Generic;
using System;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal class EnemySettings
    {
        public List<EnemySettingsGroup> enemies = new();
    }
}