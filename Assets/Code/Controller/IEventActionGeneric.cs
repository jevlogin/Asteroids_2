using System;


namespace WORLDGAMEDEVELOPMENT
{
    internal interface IEventActionGeneric<T> : IEventAction
    {
        abstract event Action<T> AddScoreByAsteroidDead;
    }
}