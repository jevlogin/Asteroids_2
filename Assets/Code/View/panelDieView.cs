using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class panelDieView : PanelView
    {
        [SerializeField] private TextMeshProUGUI _infoScoreText;
        [SerializeField] private Button _buttonSignIn;
        [SerializeField] private Button _buttonRegister;
        [SerializeField] private Button _buttonContinue;


        public Button ButtonContinue => _buttonContinue;
        public Button ButtonSignIn => _buttonSignIn;
        public Button ButtonRegister => _buttonRegister;
        public TextMeshProUGUI InfoScoreText => _infoScoreText;
    }
}