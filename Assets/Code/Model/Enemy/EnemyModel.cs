using System;

namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class EnemyModel : IAddedModel
    {
        public EnemyStruct EnemyStruct;
        public EnemyComponents EnemyComponents;
        public EnemySettings EnemySettings;

        public EnemyModel(EnemyStruct enemyStruct, EnemyComponents enemyComponents, EnemySettings enemySettings)
        {
            EnemyStruct = enemyStruct;
            EnemyComponents = enemyComponents;
            EnemySettings = enemySettings;
        }
    }
}