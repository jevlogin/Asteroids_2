using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class EnemyInitialization
    {
        private readonly EnemyFactory _enemyFactory;
        private EnemyModel _model;

        public EnemyInitialization(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            Model = _enemyFactory.GetOrCreateEnemyModel();
        }

        internal EnemyModel Model { get => _model; private set => _model = value; }
    }
}