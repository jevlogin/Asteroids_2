using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PCInputVertical : IUserInputProxy
    {
        public event Action<float> AxisOnChange;

        public void GetAxis()
        {
            AxisOnChange?.Invoke(Input.GetAxis(AxisManager.VERTICAL));
        }
    }
}