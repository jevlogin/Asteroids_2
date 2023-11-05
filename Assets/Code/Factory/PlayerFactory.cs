using System.Linq;
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

        #endregion


        #region Methods

        internal PlayerModel CreatePlayerModel(Transform startSpaceTransform)
        {
            if (_playerModel == null)
            {
                var playerStruct = _playerData.PlayerStruct;
                var playerComponents = new PlayerComponents();
                var playerSettings = new PlayerSettings();
                playerSettings.ConfigParticlesShip = _playerData.PlayerSettings.ConfigParticlesShip;
                playerSettings.ConfigParticlesShipDefault = _playerData.PlayerSettings.ConfigParticlesShipDefault;
                playerSettings.TimeForShipToTakeOff = _playerData.PlayerSettings.TimeForShipToTakeOff;
                playerSettings.SpeedAtTakeShip = _playerData.PlayerSettings.SpeedAtTakeShip;


                playerStruct.Player = CreatePlayer(ManagerName.PLAYER);
                playerStruct.Player.transform.localPosition = startSpaceTransform.position;

                playerComponents.RigidbodyPlayer = playerStruct.Player.gameObject.GetOrAddComponent<Rigidbody2D>();

                playerStruct.Player.Health = new Health(_playerData.PlayerSettings.Health);
                playerStruct.Player.Speed = new Speed(_playerData.PlayerSettings.Speed);
                playerStruct.Player.Damage = _playerData.PlayerSettings.Damage;
                playerStruct.Player.Force = _playerData.PlayerSettings.Force;

                playerStruct.SpeedScale = playerStruct.RealSpeedShipModel / playerStruct.Player.Speed.CurrentSpeed;

                var barrel = new GameObject("Barrel");
                barrel.transform.SetParent(playerStruct.Player.transform);
                float barrelPositionY = 0.0f;
                if (playerStruct.Player.transform.TryGetComponent<CapsuleCollider>(out var collider))
                {
                    barrelPositionY = collider.height;
                }
                else if (playerStruct.Player.transform.TryGetComponent<CapsuleCollider2D>(out var collider2D))
                {
                    barrelPositionY = collider2D.size.y;
                }

                barrel.transform.localPosition = new Vector2(_playerData.PlayerSettings.OffsetVectorBurel.x, barrelPositionY + 0.2f);

                playerComponents.BarrelTransform = barrel.transform;


                var particles = Object.Instantiate(_playerData.PlayerSettings.ParticleSystem, playerStruct.Player.transform);
                particles.name = _playerData.PlayerSettings.ParticleSystem.name;

                playerComponents.PlayerTransform = playerStruct.Player.transform;
                playerComponents.PlayerView = playerStruct.Player;
                playerComponents.Particles = particles.GetComponent<ParticleSystem>();
                playerComponents.Particles.Stop();
                playerComponents.Particles.gameObject.SetActive(false);
                playerComponents.AudioSource = playerStruct.Player.gameObject.GetOrAddComponent<AudioSource>();
                playerComponents.AudioSource.playOnAwake = false;
                playerComponents.AudioSource.volume = 0.03f;


                var energiyaGroupObject = playerStruct.Player.GroupObjects.FirstOrDefault(block => block.ViewObjectType == ViewObjectType.AdditionalType);
                playerComponents.RigidbodyEnergyBlock = energiyaGroupObject.Transform.gameObject.GetOrAddComponent<Rigidbody>();
                playerComponents.RigidbodyEnergyBlock.isKinematic = true;

                playerSettings.TransformPositionEnergyBlock = new Vector3(0.0f, 0.0f, energiyaGroupObject.Transform.position.z);

                _playerModel = new PlayerModel(playerStruct, playerComponents, playerSettings);
            }

            return _playerModel;
        }

        private Player CreatePlayer(string name)
        {
            var player = Object.Instantiate(_playerData.PlayerSettings.PlayerPrefab);
            player.name = name;
            player.tag = name;

            return player;
        }

        #endregion
    }
}