using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PanelHUDView : PanelView
    {
        [SerializeField] private Transform _panelHUD;

        internal Transform PanelHUD => _panelHUD;
    }
}