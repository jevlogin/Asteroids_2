using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class UpdateCharachtersView : PanelView
    {
        [SerializeField] internal GroupUpdateFields HealthPoints;
        [SerializeField] internal GroupUpdateFields Shield;
        [SerializeField] internal GroupUpdateFields Damage;
        [SerializeField] internal GroupUpdateFields Speed;
        [SerializeField] internal TextMeshProUGUI FreePoints;
    }

    [Serializable]
    public sealed class GroupUpdateFields
    {
        public TextMeshProUGUI TextUpdateValue;
        public Button ButtonConfirm;
    }
}