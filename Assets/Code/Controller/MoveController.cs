using System;
using System.Collections;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class MoveController : ICleanup, IExecute
    {
        private readonly IUserInputProxy _inputHorizontal;
        private readonly IUserInputProxy _inputVertical;
        private readonly SceneController _sceneController;
        private Transform _playerTransform;
        private Speed _speed;
        private Vector3 _move;

        private float _vertical;
        private float _horizontal;
        private bool _isStopControl;


        private bool _isMovingUp = false;
        private float _moveDuration;
        private float _moveStartTime;
        private Vector3 _initialPosition;

        internal event Action<bool> TheShipTookOff;

        public MoveController((IUserInputProxy inputHorizontal, IUserInputProxy inputVertical) getInput, Transform playerTransform, Speed speed, SceneController sceneController)
        {
            _inputHorizontal = getInput.inputHorizontal;
            _inputVertical = getInput.inputVertical;
            _playerTransform = playerTransform;
            _speed = speed;
            _inputHorizontal.AxisOnChange += HorizontalAxisOnChange;
            _inputVertical.AxisOnChange += VerticalAxisOnChange;

            _sceneController = sceneController;
            _sceneController.IsStopControl += OnChangeIsStopControl;
            _sceneController.TakeOffOfTheShip += OnChangeTakeOffOfTheShip;

        }

        private void OnChangeTakeOffOfTheShip(bool value)
        {
            //TODO - вынести в поля
            MoveUpForDuration(_speed.MaxSpeed);
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
                    float distanceToMove = _speed.CurrentSpeed * deltaTime;
                    Vector3 targetPosition = _playerTransform.position + new Vector3(0f, distanceToMove, 0f);
                    _playerTransform.position = Vector3.Lerp(_playerTransform.position, targetPosition, t);
                }
                else
                {
                    _isMovingUp = false;
                    TheShipTookOff?.Invoke(true);
                }
            }
            else
            {
                var speed = _speed.CurrentSpeed * deltaTime;
                _move.Set(_horizontal * speed, _vertical * speed, 0.0f);
                _playerTransform.localPosition += _move;
            }
        }

        private void MoveUpForDuration(float duration)
        {
            if (_isMovingUp) return;

            _isMovingUp = true;
            _moveDuration = duration;
            _moveStartTime = Time.time;
        }
    }
}