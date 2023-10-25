using System;
using System.IO;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data", order = 51)]
    internal sealed class Data : ScriptableObject
    {
        [SerializeField] private string _playerDataPath;
        [SerializeField] private string _ammunitionDataPath;
        [SerializeField] private string _enemyDataPath;

        private PlayerData _playerData;
        private AmmunitionData _ammunitionData;
        private EnemyData _enemyData;


        public EnemyData EnemyData
        {
            get
            {
                if (_enemyData == null)
                {
                    _enemyData = Resources.Load<EnemyData>(Path.Combine(ManagerPath.DATA, _enemyDataPath));
                    if (_enemyData == null)
                    {
                        _enemyData = Resources.Load<EnemyData>(Path.Combine(_enemyDataPath));
                    }
                    if (_enemyData == null)
                    {
                        AssetNotFound(nameof(_enemyData));
                    }
                }
                return _enemyData;
            }
        }

        public PlayerData PlayerData
        {
            get
            {
                if(_playerData == null)
                {
                    _playerData = Resources.Load<PlayerData>(Path.Combine(ManagerPath.DATA, _playerDataPath));
                    if(_playerData == null )
                    {
                        _playerData = Resources.Load<PlayerData>(Path.Combine(_playerDataPath));
                    }
                    if(_playerData == null )
                    {
                        AssetNotFound(nameof(_playerData));
                    }
                }
                return _playerData;
            }
        }

        public AmmunitionData AmmunitionData
        {
            get
            {
                if(_ammunitionData == null)
                {
                    _ammunitionData = Resources.Load<AmmunitionData>(Path.Combine(ManagerPath.DATA, _ammunitionDataPath));
                    if (_ammunitionData == null)
                    {
                        _ammunitionData = Resources.Load<AmmunitionData>(Path.Combine(_ammunitionDataPath));
                    }
                    if (_ammunitionData == null)
                    {
                        AssetNotFound(nameof(_ammunitionData));
                    }
                }
                return _ammunitionData;
            }
        }

        
        #region AssetNotFound
        
        private void AssetNotFound(string name)
        {
            throw new ArgumentNullException(name, "Такого ассета не существует");
        } 

        #endregion
    }
}