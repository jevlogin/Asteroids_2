using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PCInputHorizontal : IUserInputProxy
    {
        public event Action<float> AxisOnChange;

        public void GetAxis()
        {
            AxisOnChange?.Invoke(Input.GetAxis(AxisManager.HORIZONTAL));
        }
    }
}