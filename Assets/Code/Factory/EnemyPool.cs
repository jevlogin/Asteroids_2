using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class EnemyPool : GenericObjectPool<EnemyView>
    {
        private EnemySettingsGroup _enemyGroup;

        public EnemyPool(Pool<EnemyView> poolShip, Transform transformParent, EnemySettingsGroup enemyGroup) : base(poolShip, transformParent)
        {
            _enemyGroup = enemyGroup;
        }

        public EnemySettingsGroup EnemyGroup => _enemyGroup; 
    }
}