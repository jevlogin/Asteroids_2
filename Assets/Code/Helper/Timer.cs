using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class Timer : IExecute
    {
        private int _hours;
        private int _minutes;
        private int _seconds;
        private int _secondsPrev;
        private float elapsedTime = 0f;

        public event Action<string> OnChangeFullTime;
        public event Action<string> OnChangeTimeMinutes;

        public void Execute(float deltatime)
        {
            elapsedTime += Time.deltaTime;

            _hours = Mathf.FloorToInt(elapsedTime / 3600);
            _minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
            _seconds = Mathf.FloorToInt(elapsedTime % 60);

            if (MathF.Abs(_seconds - _secondsPrev) >= 1)
            {
                _secondsPrev = _seconds;
                var fullTime = string.Format("{0} часов {1} минут {2:00} секунд", _hours, _minutes, _seconds);
                var minutes = string.Format("{0} минут {1:00} секунд", _minutes, _seconds);
                OnChangeFullTime?.Invoke(fullTime);
                OnChangeTimeMinutes?.Invoke(minutes);
            }
        }
    }
}