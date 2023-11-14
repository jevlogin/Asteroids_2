using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public class GroupFieldUI
    {
        [SerializeField] internal TextMeshProUGUI CurrentField;
        [SerializeField] internal TextMeshProUGUI MaxField;

        [SerializeField] private Slider _sliderHealth;

        internal void Update(float maxValue, float currentValue)
        {
            var sliderValue = currentValue / maxValue;
            if (sliderValue < 0)
                sliderValue = 0;
            else if (sliderValue > 1)
                sliderValue = 1;

            _sliderHealth.value = sliderValue;
            CurrentField.text = currentValue.ToString();
            MaxField.text = maxValue.ToString();
        }
    }
}