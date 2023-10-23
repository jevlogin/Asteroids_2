using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class PlayerFactory
    {
        #region Fields
        
        private readonly PlayerData _playerData;
        private PlayerModel _playerModel;

        #endregion


        #region ClassLifeCycles

        public PlayerFactory(PlayerData playerData)
        {
            _playerData = playerData;
        }

        internal PlayerModel CreatePlayerModel()
        {
            if (_playerModel == null)
            {
                var playerStruct = _playerData.PlayerStruct;
                var playerComponents = new PlayerComponents();
                var playerSettings = new PlayerSettings();

                var spawnPlayer = CreatePlayer("Player");
                spawnPlayer.tag = "Player";

                var barrel = new GameObject("Barrel");
                playerComponents.BarrelTransform = barrel.transform;
                barrel.transform.SetParent(spawnPlayer.transform);
                
                barrel.transform.localPosition = new Vector2(_playerData.PlayerSettings.OffsetVectorBurel.x, 
                                                        _playerData.PlayerSettings.OffsetVectorBurel.y);

                

                var particles = Object.Instantiate(_playerData.PlayerSettings.ParticleSystem, spawnPlayer.transform);
                particles.name = _playerData.PlayerSettings.ParticleSystem.name;

                playerComponents.PlayerTransform = spawnPlayer.transform;
                playerComponents.PlayerView = spawnPlayer;
                
                _playerModel = new PlayerModel(playerStruct, playerComponents, playerSettings);
            }

            return _playerModel;
        }

        private PlayerView CreatePlayer(string name)
        {
            var player = new GameObject(name)
                .AddSprite(_playerData.PlayerSettings.SpritePlayer)
                .AddCircleCollider2D()
                .AddComponent<PlayerView>();

            return player;
        }

        #endregion



    }
}