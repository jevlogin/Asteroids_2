using System;
using System.Linq;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class ParticleController : ICleanup
    {
        private readonly PlayerModel _playerModel;
        private readonly SceneController _sceneController;
        private readonly GroupObject _energyGroup;
        private readonly GroupObject _shipGroup;
        private readonly PlayerController _playerController;

        public ParticleController(PlayerModel playerModel, SceneController sceneController, PlayerController playerController)
        {
            _playerModel = playerModel;
            _sceneController = sceneController;
            foreach (var group in _playerModel.PlayerStruct.Player.GroupObjects)
            {
                switch (group.ViewObjectType)
                {
                    case ViewObjectType.AdditionalType:
                        _energyGroup = group;
                        break;
                    case ViewObjectType.Base:
                        _shipGroup = group;
                        break;
                }
            }
            _sceneController.TakeOffOfTheShip += OnChangeTakeOffOfTheShip;
            _playerController = playerController;
            _playerController.MoveController.OnChangeBlockReset += OnChangeBlockReset;

        }

        private void OnChangeBlockReset()
        {
            ChangeParticle(false);
        }

        public void Cleanup()
        {
            _sceneController.TakeOffOfTheShip -= OnChangeTakeOffOfTheShip;
            _playerController.MoveController.OnChangeBlockReset -= OnChangeBlockReset;

        }

        private void OnChangeTakeOffOfTheShip(bool value)
        {
            ChangeParticle(value);
        }

        private void ChangeParticle(bool value)
        {
            if (value)
            {
                foreach (var particle in _energyGroup.ParticleSystems)
                {
                    ParticleSystem.MainModule main = particle.main;
                    main.startColor = _playerModel.Settings.ConfigParticlesShip.StartColor;
                    main.startSpeed = _playerModel.Settings.ConfigParticlesShip.StartSpeed;
                    main.startLifetime = _playerModel.Settings.ConfigParticlesShip.StartLifetime;
                } 
            }
            else
            {
                foreach (var particle in _energyGroup.ParticleSystems)
                {
                    ParticleSystem.MainModule main2 = particle.main;
                    main2.startColor = _playerModel.Settings.ConfigParticlesShipDefault.StartColor;
                    main2.startSpeed = _playerModel.Settings.ConfigParticlesShipDefault.StartSpeed;
                    main2.startLifetime = _playerModel.Settings.ConfigParticlesShipDefault.StartLifetime;
                }
            }
        }
    }
}