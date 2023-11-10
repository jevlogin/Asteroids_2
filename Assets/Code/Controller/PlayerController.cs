using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class PlayerController : ICleanup, IExecute, IFixedExecute, ILateExecute, IAddedModel, IInitialization
    {
        #region Fields

        private readonly InputInitialization _inputInitialization;
        private readonly PlayerInitialization _playerInitialization;
        private readonly Camera _camera;
        private readonly SceneController _sceneController;
        private readonly CanvasView _canvasView;
        private MoveController _moveController;
        private RotationController _rotationController;
        private List<IController> _controllers;
        private bool _stopControl;
        private float _timeFreezeDead;

        #endregion


        #region Properties

        internal RotationController RotationController
        {
            get
            {
                if (_rotationController == null)
                {
                    _rotationController = new RotationController(_inputInitialization.GetInput(), _playerInitialization.PlayerModel.Components.PlayerTransform,
                                                _camera, _sceneController, _playerInitialization.PlayerModel.Components.RigidbodyPlayer);
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
                    _moveController = new MoveController(_inputInitialization.GetInput(), _playerInitialization.PlayerModel.Components.RigidbodyPlayer,
                                               _playerInitialization.PlayerModel.Components.PlayerTransform,
                                               _playerInitialization.PlayerModel.PlayerStruct.Player.Speed,
                                               _playerInitialization.PlayerModel,
                                               _sceneController, _canvasView.panelViews);
                }
                return _moveController;
            }
        }

        #endregion


        #region ClassLifeCycles

        public PlayerController(InputInitialization inputInitialization, PlayerInitialization playerInitialization, Camera camera, SceneController sceneController, CanvasView canvasView)
        {
            _inputInitialization = inputInitialization;
            _playerInitialization = playerInitialization;
            _camera = camera;
            _sceneController = sceneController;
            _sceneController.IsStopControl += OnCnageIsStopControl;
            _sceneController.DisableEnergyBlock += DisableEnergyBlock;
            _playerInitialization.PlayerModel.PlayerStruct.Player.EnableShield += EnableShield;

            _canvasView = canvasView;

            _playerInitialization.PlayerModel.PlayerStruct.Player.IsDeadPlayer += IsDeadPlayerAndRestartPosition;

            _controllers = new()
            {
                MoveController,
                RotationController,
            };

            MoveController.OnChangeBlockReset += OnChangeBlockReset;
            MoveController.DisableEnergyBlock += DisableEnergyBlock;
        }

        private void EnableShield()
        {
            _playerInitialization.PlayerModel.Components.ShieldView.PlayShield();
        }

        private void IsDeadPlayerAndRestartPosition()
        {
            _stopControl = true;
            _playerInitialization.PlayerModel.PlayerStruct.Player.transform.position = new Vector3(0, 0, 0);
            _playerInitialization.PlayerModel.PlayerStruct.Player.transform.rotation = Quaternion.identity;
            _timeFreezeDead = 0.0f;
        }

        private void DisableEnergyBlock()
        {
            ParticleSystem.MainModule mainModule = _playerInitialization.PlayerModel.Components.ParticlesStarSystem.main;
            mainModule.gravityModifier = _playerInitialization.PlayerModel.PlayerStruct.ParticleSpeedAfterTakeOff; 
            
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.gameObject.SetActive(false);
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.transform.SetParent(_playerInitialization.PlayerModel.Components.PlayerTransform);

            var position = _playerInitialization.PlayerModel.Components.PlayerTransform.position + _playerInitialization.PlayerModel.Settings.TransformPositionEnergyBlock;

            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.transform.position = position;
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.transform.rotation = _playerInitialization.PlayerModel.Components.PlayerTransform.rotation;
            _playerInitialization.PlayerModel.Components.RigidbodyEnergyBlock.isKinematic = true;
        }

        private void OnChangeBlockReset()
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
            if (!_playerInitialization.PlayerModel.Components.ParticlesStarSystem.gameObject.activeSelf)
            {
                _playerInitialization.PlayerModel.Components.ParticlesStarSystem.gameObject.SetActive(true);
            }
            if (_playerInitialization.PlayerModel.Components.ParticlesStarSystem.isStopped
                    || _playerInitialization.PlayerModel.Components.ParticlesStarSystem.isPaused)
            {
                _playerInitialization.PlayerModel.Components.ParticlesStarSystem.Play();
            }
        }

        #endregion


        #region ICleanup

        public void Cleanup()
        {
            _moveController.Cleanup();
            _playerInitialization.PlayerModel.PlayerStruct.Player.EnableShield -= EnableShield;
            MoveController.OnChangeBlockReset -= OnChangeBlockReset;
            _sceneController.IsStopControl -= OnCnageIsStopControl;
            _sceneController.DisableEnergyBlock -= DisableEnergyBlock;
            MoveController.DisableEnergyBlock -= DisableEnergyBlock;
        }

        #endregion


        #region IExecute

        public void Execute(float deltatime)
        {
            if (!_stopControl)
            {
                foreach (var controller in _controllers)
                {
                    if (controller is IExecute execute)
                    {
                        execute.Execute(deltatime);
                    }
                }
            }
            else
            {
                _timeFreezeDead += Time.deltaTime;
                if (_timeFreezeDead > 3.0f)
                {
                    _stopControl = false;
                }
                else if (_timeFreezeDead > 1.0f && !_playerInitialization.PlayerModel.PlayerStruct.Player.gameObject.activeSelf)
                {
                    _playerInitialization.PlayerModel.PlayerStruct.Player.IsCanShoot?.Invoke(true);
                    _playerInitialization.PlayerModel.PlayerStruct.Player.gameObject.SetActive(true);
                }
            }
        }

        public void LateExecute(float deltatime)
        {
            foreach (var controller in _controllers)
            {
                if (controller is ILateExecute execute)
                {
                    execute.LateExecute(deltatime);
                }
            }
        }

        public void FixedExecute(float fixedDeltatime)
        {
            foreach (var controller in _controllers)
            {
                if (controller is IFixedExecute execute)
                {
                    execute.FixedExecute(fixedDeltatime);
                }
            }
        }

        public void Initialization()
        {
            foreach (var controller in _controllers)
            {
                if (controller is IInitialization init)
                {
                    init.Initialization();
                }
            }
        }

        #endregion
    }
}