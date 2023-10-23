using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class MoveController : ICleanup, IExecute
    {
        private readonly IUserInputProxy _inputHorizontal;
        private readonly IUserInputProxy _inputVertical;
        private Transform _playerTransform;
        private Speed _speed;
        private Vector3 _move;

        private float _vertical;
        private float _horizontal;

        public MoveController((IUserInputProxy inputHorizontal, IUserInputProxy inputVertical) getInput, Transform playerTransform, Speed speed)
        {
            _inputHorizontal = getInput.inputHorizontal;
            _inputVertical = getInput.inputVertical;
            _playerTransform = playerTransform;
            _speed = speed;
            _inputHorizontal.AxisOnChange += HorizontalAxisOnChange;
            _inputVertical.AxisOnChange += VerticalAxisOnChange;
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
        }

        public void Execute(float deltatime)
        {
            var speed = _speed.CurrentSpeed * deltatime;
            _move.Set(_horizontal * speed, _vertical * speed, 0.0f);
            //TODO - _move.Normilize();
            _playerTransform.localPosition += _move;
        }
    }
}