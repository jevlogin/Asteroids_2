using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Speed
    {
        [SerializeField, Range(1, 100), Tooltip("Максимальная скорость")] private float _maxSpeed;
        [SerializeField, Range(0, 100), Tooltip("Текущая скорость")] private float _currentSpeed;
        [SerializeField, Range(1, 30), Tooltip("Ускорение")] private float _acceleration;

        public Speed(Speed speed)
        {
            _maxSpeed = speed.MaxSpeed;
            _currentSpeed = speed.CurrentSpeed;
            _acceleration = speed.Acceleration;
        }

        public float Acceleration { get => _acceleration; }
        public float CurrentSpeed { get => _currentSpeed; }
        public float MaxSpeed { get => _maxSpeed;}
    }
}