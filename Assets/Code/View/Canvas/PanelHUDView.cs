using TMPro;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PanelHUDView : PanelView
    {
        [SerializeField] private Transform _panelHUD;
        [SerializeField] private TextMeshProUGUI _textScore;
        [SerializeField] private TextMeshProUGUI _textLife;

        public GroupFieldUI Health;
        public GroupFieldUI Shield;

        internal float Score = 0.0f;


        internal Transform PanelHUD => _panelHUD;
        internal TextMeshProUGUI TextScore => _textScore;
        internal TextMeshProUGUI TextLife => _textLife;
    }
}