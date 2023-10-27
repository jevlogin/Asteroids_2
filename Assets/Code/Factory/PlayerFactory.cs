using Unity.VisualScripting;
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
                var playerStruct = new PlayerStruct();
                var playerComponents = new PlayerComponents();
                var playerSettings = new PlayerSettings();

                playerStruct.Player = CreatePlayer("Player");
                playerStruct.Player.tag = "Player";

                playerStruct.Player.Health = new Health(_playerData.PlayerSettings.Health);
                playerStruct.Player.Speed = new Speed(_playerData.PlayerSettings.Speed);
                playerStruct.Player.Damage = _playerData.PlayerSettings.Damage;
                playerStruct.Player.Force = _playerData.PlayerSettings.Force;

                var barrel = new GameObject("Barrel");
                playerComponents.BarrelTransform = barrel.transform;
                barrel.transform.SetParent(playerStruct.Player.transform);
                barrel.transform.localPosition = new Vector2(_playerData.PlayerSettings.OffsetVectorBurel.x, 
                                                        _playerData.PlayerSettings.OffsetVectorBurel.y);

                var particles = Object.Instantiate(_playerData.PlayerSettings.ParticleSystem, playerStruct.Player.transform);
                particles.name = _playerData.PlayerSettings.ParticleSystem.name;

                playerComponents.PlayerTransform = playerStruct.Player.transform;
                playerComponents.PlayerView = playerStruct.Player;
                
                _playerModel = new PlayerModel(playerStruct, playerComponents, playerSettings);
            }

            return _playerModel;
        }

        private Player CreatePlayer(string name)
        {
            var player = new GameObject(name)
                .AddSprite(_playerData.PlayerSettings.SpritePlayer)
                .AddCircleCollider2D()
                .AddComponent<Player>();

            return player;
        }

        #endregion
    }
}