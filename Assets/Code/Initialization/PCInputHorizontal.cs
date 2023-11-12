using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PCInputHorizontal : IUserInputProxy
    {
        private InputAction _moveLeftAndRightInput;
        private float _horizontal;

        public event Action<float> AxisOnChange;

        public PCInputHorizontal(InputAction moveLeftAndRight)
        {
            _moveLeftAndRightInput = moveLeftAndRight;
            _moveLeftAndRightInput.Enable();
            _moveLeftAndRightInput.performed += _moveLeftAndRightInput_performed;
            _moveLeftAndRightInput.canceled += _moveLeftAndRightInput_canceled;
        }

        private void _moveLeftAndRightInput_canceled(InputAction.CallbackContext context)
        {
            _horizontal = context.ReadValue<Vector2>().x;
        }

        private void _moveLeftAndRightInput_performed(InputAction.CallbackContext context)
        {
            _horizontal = context.ReadValue<Vector2>().x;
        }

        public void GetAxis()
        {
            AxisOnChange?.Invoke(_horizontal);
        }
    }
}