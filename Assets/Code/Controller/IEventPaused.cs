using System;


namespace WORLDGAMEDEVELOPMENT
{
    internal interface IEventPaused
    {
        event Action<bool> OnPause;
    }
}