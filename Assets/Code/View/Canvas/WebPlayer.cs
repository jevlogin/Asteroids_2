using System;
using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class WebPlayer
    {
        [SerializeField] private Button _buttonPrev;
        [SerializeField] private Button _buttonPlay;
        [SerializeField] private Button _buttonPause;
        [SerializeField] private Button _buttonNext;

        public Button ButtonPrev  => _buttonPrev;
        public Button ButtonPlay  => _buttonPlay; 
        public Button ButtonPause => _buttonPause;
        public Button ButtonNext => _buttonNext;
    }
}