using System;


namespace WORLDGAMEDEVELOPMENT
{
    internal interface IUserInputBool
    {
        event Action<bool> OnInputBoolOnChange;
        void GetButtonDown();
    }
}