using System.Collections.Generic;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class PlayerController : IInitialization, ICleanup, IExecute
    {
        #region Fields

        private readonly InputInitialization _inputInitialization;
        private readonly PlayerInitialization _playerInitialization;
        private readonly Camera _camera;
        private MoveController _moveController;
        private RotationController _rotationController;
        private List<IController> _controllers;

        #endregion


        #region ClassLifeCycles

        public PlayerController(InputInitialization inputInitialization, PlayerInitialization playerInitialization, Camera camera)
        {
            _inputInitialization = inputInitialization;
            _playerInitialization = playerInitialization;
            _camera = camera;
        }

        #endregion


        #region ICleanup

        public void Cleanup()
        {
            _moveController.Cleanup();
        }

        #endregion


        #region IExecute

        public void Execute(float deltatime)
        {
            foreach (var controller in _controllers)
            {
                if(controller is IExecute execute)
                {
                    execute.Execute(deltatime);
                }
            }
        }

        #endregion


        #region IInitialization

        public void Initialization()
        {
            _controllers = new List<IController>();
            _moveController = new MoveController(_inputInitialization.GetInput(),
                                                _playerInitialization.PlayerModel.Components.PlayerTransform,
                                                _playerInitialization.PlayerModel.PlayerStruct.Speed);
            _rotationController = new RotationController(_playerInitialization.PlayerModel.Components.PlayerTransform, _camera);
            
            _controllers.Add(_moveController);
            _controllers.Add(_rotationController);
        }

        #endregion
    }
}