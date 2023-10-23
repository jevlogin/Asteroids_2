using System;


namespace WORLDGAMEDEVELOPMENT
{
    internal interface IUserInputProxy
    {
        event Action<float> AxisOnChange;
        void GetAxis();
    }
}