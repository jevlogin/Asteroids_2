using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal interface IAmmunition
    {
        Rigidbody2D Rigidbody2D { get; }
        float TimeDestroy { get; }
    }
}