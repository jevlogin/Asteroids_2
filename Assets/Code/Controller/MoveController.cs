using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class MoveController : ICleanup, IExecute, IFixedExecute, IEventActionGeneric<float>
    {
        private readonly IUserInputProxy _inputHorizontal;
        private readonly IUserInputProxy _inputVertical;
        private readonly SceneController _sceneController;
        private Transform _playerTransform;
        private Speed _speed;
        private readonly Rigidbody2D _rigidbodyPlayer;
        private readonly PlayerModel _playerModel;
        private Vector3 _move;

        private float _vertical;
        private float _horizontal;
        private bool _isStopControl;


        private bool _isMovingUp = false;
        private float _moveDuration;
        private float _moveStartTime;
        private float _currentStatePosition;
        private float _epsilon = 0.5f;
        private bool _isMovingFreeControl;

        internal event Action<bool> TheShipTookOff;

        internal event Action<float> OnChangeSpeedMovement;
        public event Action<float> OnChangePositionRelativeToAxisY;


        public MoveController((IUserInputProxy inputHorizontal, IUserInputProxy inputVertical) getInput, Rigidbody2D rigidbodyPlayer, Transform playerTransform, Speed speed, PlayerModel playerModel, SceneController sceneController)
        {
            _inputHorizontal = getInput.inputHorizontal;
            _inputVertical = getInput.inputVertical;
            _playerTransform = playerTransform;
            _speed = speed;
            _rigidbodyPlayer = rigidbodyPlayer;
            _playerModel = playerModel;

            _inputHorizontal.AxisOnChange += HorizontalAxisOnChange;
            _inputVertical.AxisOnChange += VerticalAxisOnChange;
            _sceneController = sceneController;
            _sceneController.IsStopControl += OnChangeIsStopControl;
            _sceneController.TakeOffOfTheShip += OnChangeTakeOffOfTheShip;
        }

        private void OnChangeTakeOffOfTheShip(bool value)
        {
            MoveUpForDuration(_playerModel.Settings.TimeForShipToTakeOff);
        }

        private void OnChangeIsStopControl(bool value)
        {
            _isStopControl = value;
        }

        private void VerticalAxisOnChange(float value)
        {
            _vertical = value;
        }

        private void HorizontalAxisOnChange(float value)
        {
            _horizontal = value;
        }

        public void Cleanup()
        {
            _inputHorizontal.AxisOnChange -= HorizontalAxisOnChange;
            _inputVertical.AxisOnChange -= VerticalAxisOnChange;
            _sceneController.IsStopControl -= OnChangeIsStopControl;
            _sceneController.TakeOffOfTheShip -= OnChangeTakeOffOfTheShip;
        }

        public void Execute(float deltaTime)
        {
            if (_isStopControl && !_isMovingUp)
                return;

            if (_isMovingUp)
            {
                float elapsedTime = Time.time - _moveStartTime;

                if (elapsedTime < _moveDuration)
                {
                    float t = elapsedTime / _moveDuration;
                    float distanceToMove = _playerModel.Settings.SpeedAtTakeShip * deltaTime;

                    Vector3 targetPosition = _playerTransform.position + new Vector3(0f, distanceToMove, 0f);
                    _playerTransform.position = Vector3.Lerp(_playerTransform.position, targetPosition, t);
                }
                else
                {
                    _isMovingUp = false;
                    _isMovingFreeControl = true;
                    TheShipTookOff?.Invoke(true);
                }
            }
        }

        private void MoveUpForDuration(float duration)
        {
            if (_isMovingUp) return;

            _isMovingFreeControl = false;
            _isMovingUp = true;
            _moveDuration = duration;
            _moveStartTime = Time.time;
        }

        public void FixedExecute(float fixedDeltatime)
        {
            if (Mathf.Abs(_playerTransform.position.y - _currentStatePosition) > _epsilon)
            {
                _currentStatePosition = _playerTransform.position.y;
                float realHeight = _currentStatePosition * _playerModel.PlayerStruct.RealSpeedShipModel * fixedDeltatime;
                OnChangePositionRelativeToAxisY?.Invoke(realHeight);
            }

            if (_isMovingFreeControl)
            {
                var movement = new Vector2(_horizontal, _vertical).normalized;
                movement *= _speed.CurrentSpeed * fixedDeltatime;
                var newVelocity = Vector2.Lerp(_rigidbodyPlayer.velocity, movement, _playerModel.PlayerStruct.VelocityChangeSpeed * fixedDeltatime);
                _rigidbodyPlayer.velocity = newVelocity;
                var gameSpeed = _playerModel.PlayerStruct.SpeedScale * newVelocity.sqrMagnitude * _playerModel.PlayerStruct.ScaleFactor;
                OnChangeSpeedMovement?.Invoke(gameSpeed);
            }
        }
    }
}