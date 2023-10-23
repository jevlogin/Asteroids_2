using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PCInputMouse : IUserInputBool
    {
        public event Action<bool> OnInputBoolOnChange;

        public void GetButtonDown()
        {
            OnInputBoolOnChange?.Invoke(Input.GetButtonDown(AxisManager.FIRE1));
        }
    }
}