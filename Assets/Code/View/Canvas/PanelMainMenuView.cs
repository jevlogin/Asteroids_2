using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PanelMainMenuView : PanelView
    {
        #region Fields

        [SerializeField] private Button _buttonStart;
        [SerializeField] private Button _buttonConfig;
        [SerializeField] private Button _buttonQuit;

        #endregion


        #region Properties

        public Button ButtonStart => _buttonStart;
        public Button ButtonConfig => _buttonConfig;
        public Button ButtonQuit => _buttonQuit;

        #endregion
    }
}