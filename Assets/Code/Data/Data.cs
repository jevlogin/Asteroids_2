using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data", order = 51)]
    internal sealed class Data : ScriptableObject
    {
        [SerializeField] private string _playerDataPath;
        [SerializeField] private string _ammunitionDataPath;
        [SerializeField] private string _enemyDataPath;
        [SerializeField] private string _sceneDataPath;
        [SerializeField] private string _canvasDataPath;
        [SerializeField] private string _audioDataPath;
        [SerializeField] private string _vFXDataPath;
        [SerializeField] private string _backgroundDataPath;

        private PlayerData _playerData;
        private AmmunitionData _ammunitionData;
        private EnemyData _enemyData;
        private SceneData _sceneData;
        private CanvasData _canvasData;
        private AudioData _audioData;
        private VFXData _vFXData;
        private BackgroundData _backgroundData;


        public BackgroundData BackgroundData
        {
            get
            {
                if (_backgroundData == null)
                {
                    _backgroundData = Resources.Load<BackgroundData>(Path.Combine(ManagerPath.DATA, _backgroundDataPath));
                    if (_backgroundData == null)
                        _backgroundData = Resources.Load<BackgroundData>(Path.Combine(_backgroundDataPath));
                    if (_backgroundData == null)
                        AssetNotFound(nameof(_backgroundData));
                }
                return _backgroundData;
            }
        }

        public VFXData VFXData
        {
            get
            {
                if (_vFXData == null)
                {
                    _vFXData = Resources.Load<VFXData>(Path.Combine(ManagerPath.DATA, _vFXDataPath));
                    if (_vFXData == null)
                        _vFXData = Resources.Load<VFXData>(_vFXDataPath);
                    if (_vFXData == null)
                        AssetNotFound(nameof(_vFXData));
                }
                return _vFXData;
            }
        }

        public AudioData AudioData
        {
            get
            {
                if (_audioData == null)
                {
                    _audioData = Resources.Load<AudioData>(Path.Combine(ManagerPath.DATA, _audioDataPath));
                    if (_audioData == null)
                        _audioData = Resources.Load<AudioData>(Path.Combine(_audioDataPath));
                    if (_audioData == null)
                        AssetNotFound(nameof(_audioData));
                }
                return _audioData;
            }
        }



        public CanvasData CanvasData
        {
            get
            {
                if (_canvasData == null)
                {
                    _canvasData = Resources.Load<CanvasData>(Path.Combine(ManagerPath.DATA, _canvasDataPath));
                    if (_canvasData == null)
                        _canvasData = Resources.Load<CanvasData>(Path.Combine(_canvasDataPath));
                    if (_canvasData == null)
                        AssetNotFound(nameof(_canvasData));
                }
                return _canvasData;
            }
        }

        public SceneData SceneData
        {
            get
            {
                if (_sceneData == null)
                {
                    _sceneData = Resources.Load<SceneData>(Path.Combine(ManagerPath.DATA, _sceneDataPath));
                    if (_sceneData == null)
                        _sceneData = Resources.Load<SceneData>(Path.Combine(_sceneDataPath));
                    if (_sceneData == null)
                        AssetNotFound(nameof(_sceneData));
                }

                return _sceneData;
            }
        }



        public EnemyData EnemyData
        {
            get
            {
                if (_enemyData == null)
                {
                    _enemyData = Resources.Load<EnemyData>(Path.Combine(ManagerPath.DATA, _enemyDataPath));
                    if (_enemyData == null)
                        _enemyData = Resources.Load<EnemyData>(Path.Combine(_enemyDataPath));
                    if (_enemyData == null)
                        AssetNotFound(nameof(_enemyData));
                }
                return _enemyData;
            }
        }

        public PlayerData PlayerData
        {
            get
            {
                if (_playerData == null)
                {
                    _playerData = Resources.Load<PlayerData>(Path.Combine(ManagerPath.DATA, _playerDataPath));
                    if (_playerData == null)
                        _playerData = Resources.Load<PlayerData>(Path.Combine(_playerDataPath));
                    if (_playerData == null)
                        AssetNotFound(nameof(_playerData));
                }
                return _playerData;
            }
        }

        public AmmunitionData AmmunitionData
        {
            get
            {
                if (_ammunitionData == null)
                {
                    _ammunitionData = Resources.Load<AmmunitionData>(Path.Combine(ManagerPath.DATA, _ammunitionDataPath));
                    if (_ammunitionData == null)
                        _ammunitionData = Resources.Load<AmmunitionData>(Path.Combine(_ammunitionDataPath));
                    if (_ammunitionData == null)
                        AssetNotFound(nameof(_ammunitionData));
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