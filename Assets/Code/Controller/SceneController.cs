using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class SceneController : IInitialization, ICleanup, IExecute
    {
        private readonly SceneModel _sceneModel;
        private List<IAddedModel> _addedModels;

        internal event Action<bool> IsStopControl;
        internal event Action<bool> IsCanPlayBackgroundMusic;
        internal event Action<bool> TakeOffOfTheShip;

        internal Action DisableEnergyBlock;
        private PanelGameMenuView _panelMenu;
        private PanelHUDView _panelHUD;
        private PlayerModel _playerModel;
        private PanelMainMenuView _panelMainMenu;
        private Timer _timerLevelLeft;
        private SceneControllerUIView _sceneControllerUIView;
        private int _currentWave;
        private bool _isTakeOffShip;
        private IEventPaused _eventPaused;

        internal int CurrentWave => _currentWave;

        internal IEnumerable<IAddedModel> AddedModels => _addedModels;

        public SceneController(SceneModel sceneModel)
        {
            _sceneModel = sceneModel;
            _addedModels = new();
            _currentWave = 1;

            _sceneModel.SceneStruct.BroadcastEventManager.OnStartGame += OnStartGame;
        }

        private void OnStartGame()
        {
            if (!_isTakeOffShip)
            {
                StartControl();
                IsCanPlayBackgroundMusic?.Invoke(true);
            }
        }

        internal void Add(IEventPaused eventPaused)
        {
            _eventPaused = eventPaused;
            _eventPaused.OnPause += OnPause;
        }

        private void OnPause(bool value)
        {
            IsStopControl?.Invoke(value);
            IsCanPlayBackgroundMusic?.Invoke(!value);
        }

        internal void Add(IAddedModel addedModel)
        {
            _addedModels.Add(addedModel);

            if (addedModel is CanvasModel canvasModel)
            {
                foreach (var panel in canvasModel.CanvasStruct.CanvasView.panelViews)
                {
                    if (panel is PanelHUDView panelHUD)
                    {
                        _panelHUD = panelHUD;
                    }
                    if (panel is PanelMainMenuView panelMainMenu)
                    {
                        _panelMainMenu = panelMainMenu;
                    }
                    if (panel is SceneControllerUIView sceneControllerUIView)
                    {
                        _sceneControllerUIView = sceneControllerUIView;
                    }
                }
            }
            if (addedModel is PlayerModel playerModel)
            {
                _playerModel = playerModel;
                _playerModel.PlayerStruct.Player.LifeLeftCount += PlayerTakeDamage;
            }
        }

        private void PlayerTakeDamage(float currentHealth)
        {
            _panelHUD.TextLife.text = currentHealth.ToString();
        }

        private void TheShipTookOff(bool value)
        {
            IsStopControl?.Invoke(false);
            _sceneModel.SceneStruct.StartSceneView.gameObject.SetActive(false);
            _playerModel.PlayerStruct.Player.IsCanShoot?.Invoke(true);
        }

        private void StartControl()
        {
            TakeOffOfTheShip?.Invoke(true);
            _isTakeOffShip = true;
        }

        public void Initialization()
        {
            IsStopControl?.Invoke(true);

            foreach (var model in AddedModels)
            {
                if (model is PlayerController playerController)
                {
                    playerController.MoveController.TheShipTookOff += TheShipTookOff;
                }
            }

            _timerLevelLeft = new Timer();
            _timerLevelLeft.OnChangeTimeMinutes += OnChangeTimeMinutes;

            _sceneControllerUIView.TextTimeWave.text = _currentWave.ToString();
        }

        private void OnChangeTimeMinutes(string time)
        {
            _sceneControllerUIView.TextTimeWave.text = time;
        }

        public void Cleanup()
        {
            _panelMenu.ButtonStart.onClick.RemoveAllListeners();
            _timerLevelLeft.OnChangeTimeMinutes -= OnChangeTimeMinutes;
            _sceneModel.SceneStruct.BroadcastEventManager.OnStartGame -= OnStartGame;
            _eventPaused.OnPause -= OnPause;
        }

        public void Execute(float deltatime)
        {
            _timerLevelLeft.Execute(deltatime);
        }
    }
}