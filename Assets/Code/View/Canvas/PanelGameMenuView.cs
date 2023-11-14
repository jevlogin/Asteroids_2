using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PanelGameMenuView : PanelView
    {
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Transform _panelMenuStart;
        [SerializeField] private TextMeshProUGUI _textLevel;

        internal TextMeshProUGUI TextLevel => _textLevel;
        internal Button ButtonStart => _buttonStart;
        internal Transform PanelMenuStart => _panelMenuStart;
    }
}