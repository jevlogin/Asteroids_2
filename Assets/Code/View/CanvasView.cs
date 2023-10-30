using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class CanvasView : MonoBehaviour
    {
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Transform _panelMenuStart;

        internal Button ButtonStart => _buttonStart;
        internal Transform PanelMenuStart => _panelMenuStart;
    }
}