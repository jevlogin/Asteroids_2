using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal interface ICollisionDetect
    {
        event Action<Collider2D> OnCollisionEnterDetect;
    }
}