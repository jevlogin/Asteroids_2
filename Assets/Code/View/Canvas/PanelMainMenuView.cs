using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PanelMainMenuView : PanelView
    {
        #region Fields

        [SerializeField] private Button _buttonQuit;

        #endregion


        #region Properties

        public Button ButtonQuit => _buttonQuit;

        #endregion
    }
}