using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class PlayerInitialization
    {
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerModel _playerModel;

        public PlayerInitialization(PlayerFactory playerFactory, Transform startSpaceTransform)
        {
            _playerFactory = playerFactory;
            _playerModel = _playerFactory.CreatePlayerModel(startSpaceTransform);
        }

        internal PlayerModel PlayerModel => _playerModel;
    }
}