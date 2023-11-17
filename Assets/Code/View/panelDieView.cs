using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class panelDieView : PanelView
    {
        [SerializeField] private Button _buttonContinue;

        public Button ButtonContinue => _buttonContinue;
    }
}