using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PanelGameMenuView : PanelView
    {
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Transform _panelMenuStart;

        internal Button ButtonStart => _buttonStart;
        internal Transform PanelMenuStart => _panelMenuStart;
    }
}