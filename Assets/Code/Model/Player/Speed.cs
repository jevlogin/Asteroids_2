using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class Speed
    {
        [SerializeField, Range(0, 100), Tooltip("Максимальная скорость")] private float _maxSpeed;
        [SerializeField, Range(0, 100), Tooltip("Текущая скорость")] private float _currentSpeed;
        [SerializeField, Range(5, 30), Tooltip("Ускорение")] private float _acceleration;

        public Speed(float speed, float maxSpeed, float acceleration)
        {
            _currentSpeed = speed;
            _maxSpeed = maxSpeed;
            _acceleration = acceleration;
        }

        public float Acceleration { get => _acceleration; }
        public float CurrentSpeed { get => _currentSpeed; }
        public float MaxSpeed { get => _maxSpeed;}
    }
}