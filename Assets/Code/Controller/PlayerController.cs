using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class PlayerController : ICleanup, IExecute, IAddedModel
    {
        #region Fields

        private readonly InputInitialization _inputInitialization;
        private readonly PlayerInitialization _playerInitialization;
        private readonly Camera _camera;
        private readonly SceneController _sceneController;
        private MoveController _moveController;
        private RotationController _rotationController;
        private List<IController> _controllers;

        #endregion


        #region Properties

        internal RotationController RotationController
        {
            get
            {
                if (_rotationController == null)
                {
                    _rotationController = new RotationController(_playerInitialization.PlayerModel.Components.PlayerTransform,
                                                _camera, _sceneController);
                }
                return _rotationController;
            }
        }

        internal MoveController MoveController
        {
            get
            {
                if (_moveController == null)
                {
                    _moveController = new MoveController(_inputInitialization.GetInput(),
                                               _playerInitialization.PlayerModel.Components.PlayerTransform,
                                               _playerInitialization.PlayerModel.PlayerStruct.Player.Speed,
                                               _sceneController);
                }
                return _moveController;
            }
        }

        #endregion


        #region ClassLifeCycles

        public PlayerController(InputInitialization inputInitialization, PlayerInitialization playerInitialization, Camera camera, SceneController sceneController)
        {
            _inputInitialization = inputInitialization;
            _playerInitialization = playerInitialization;
            _camera = camera;
            _sceneController = sceneController;
            _sceneController.IsStopControl += OnCnageIsStopControl;
            _sceneController.StartParticle += StartParticle;
            _sceneController.DisableEnergyBlock += DisableEnergyBlock;

            _controllers = new()
            {
                MoveController,
                RotationController
            };
        }

        private void DisableEnergyBlock()
        {
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.gameObject.SetActive(false);
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.transform.SetParent(_playerInitialization.PlayerModel.Components.PlayerTransform);
            
            var position = _playerInitialization.PlayerModel.Components.PlayerTransform.position + _playerInitialization.PlayerModel.Settings.TransformPositionEnergyBlock;

            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.transform.position = position;
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.transform.rotation = _playerInitialization.PlayerModel.Components.PlayerTransform.rotation;
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.isKinematic = true;
        }

        private void StartParticle()
        {
            PlayParticle();
            ActivateEnergyBlockRigidbody();
        }

        private void OnCnageIsStopControl(bool value)
        {
            if (!value)
            {
                ActivateEnergyBlockRigidbody();
            }
        }

        private void ActivateEnergyBlockRigidbody()
        {
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.transform.SetParent(null);
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.isKinematic = false;
        }

        private void PlayParticle()
        {
            if (!_playerInitialization.PlayerModel.Components.Particles.gameObject.activeSelf)
            {
                _playerInitialization.PlayerModel.Components.Particles.gameObject.SetActive(true);
            }
            if (_playerInitialization.PlayerModel.Components.Particles.isStopped
                    || _playerInitialization.PlayerModel.Components.Particles.isPaused)
            {
                _playerInitialization.PlayerModel.Components.Particles.Play();
            }
        }

        #endregion


        #region ICleanup

        public void Cleanup()
        {
            _moveController.Cleanup();
            _sceneController.StartParticle -= StartParticle;
            _sceneController.IsStopControl -= OnCnageIsStopControl;
            _sceneController.DisableEnergyBlock -= DisableEnergyBlock;
        }

        #endregion


        #region IExecute

        public void Execute(float deltatime)
        {
            foreach (var controller in _controllers)
            {
                if (controller is IExecute execute)
                {
                    execute.Execute(deltatime);
                }
            }
        }

        #endregion
    }
}