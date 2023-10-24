using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PlayerShooterController : IController
    {
        private IUserInputBool _userInputMouse;
        private PlayerInitialization _playerInitialization;
        private AmmunitionModel _ammunitionFactoryModel;
        private readonly Transform _barrelTransform;
        private bool _valueChange;
        private float _refireTimer = 0.3f;
        private float _fireTimer;
        public float MoveSpeed = 5.0f;
        private readonly float _maxLifeTime = 5.0f;


        public PlayerShooterController(IUserInputBool userInputBool, PlayerInitialization playerInitialization, AmmunitionModel ammunitionFactoryModel)
        {
            _userInputMouse = userInputBool;
            _playerInitialization = playerInitialization;
            _ammunitionFactoryModel = ammunitionFactoryModel;

            _barrelTransform = _playerInitialization.PlayerModel.Components.BarrelTransform;

            _fireTimer = _refireTimer;
        }
    }
}