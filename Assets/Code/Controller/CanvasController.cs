using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasController : IController, ICleanup, IInitialization, ILateExecute, IExecute, IEventPaused
    {
        private CanvasModel _canvasModel;
        private readonly SceneModel _sceneModel;
        private readonly PlayerModel _playerModel;
        private readonly PanelHUDView _panelHUD;
        private readonly PanelMainMenuView _panelMainMenu;
        private readonly PanelResultsView _panelResults;
        private List<IEventAction> _listEvent = new();
        private List<Button> _allButtonList = new();
        private bool _isPaused;
        private bool _isGameStarted;

        internal event Action<EventCanvas> StartGame;
        public event Action<bool> OnPause;

        private Timer _timerToLeftInGame;
        private Timer _timerLevelLeft;
        private float _distanceTravel;
        private Dictionary<PanelView, bool> _viewsStateActive;
        private readonly SceneControllerUIView _sceneControllerUIView;
        private readonly panelDieView _panelDie;

        public CanvasController(CanvasModel canvasModel, PlayerModel playerModel, SceneModel sceneModel)
        {
            _canvasModel = canvasModel;
            _sceneModel = sceneModel;
            _sceneModel.SceneStruct.BroadcastEventManager.OnStartGame += OnStartGame;

            _playerModel = playerModel;
            _playerModel.PlayerStruct.Player.Health.OnChangeHealth += OnChangeHealth;
            _playerModel.PlayerStruct.Player.Shield.OnChangeShield += OnChangeShield;
            _playerModel.PlayerStruct.Player.Expirience.OnChangeExpirience += OnChangeExpirience;
            _playerModel.PlayerStruct.Player.IsDeadPlayer += IsDeadPlayer;


            _timerToLeftInGame = new Timer();
            _timerToLeftInGame.OnChangeFullTime += OnChangeTimeToLeftInGame;


            foreach (var panel in _canvasModel.CanvasStruct.CanvasView.panelViews)
            {
                panel.gameObject.SetActive(false);

                if (panel is SceneControllerUIView sceneControllerUIView)
                {
                    _sceneControllerUIView = sceneControllerUIView;
                }
                if (panel is panelDieView panelDie)
                {
                    _panelDie = panelDie;
                    _panelDie.ButtonContinue.onClick.AddListener(ContinueAfterDead);
                }
                if (panel is PanelHUDView panelHUD)
                {
                    _panelHUD = panelHUD;
                }
                if (panel is PanelMainMenuView panelMainMenu)
                {
                    _panelMainMenu = panelMainMenu;
                }
                if (panel is PanelResultsView panelResults)
                {
                    _panelResults = panelResults;
                }
            }

            foreach (var item in _canvasModel.CanvasStruct.CanvasView.transform.GetComponentsInChildren<Button>())
            {
                _allButtonList.Add(item);
            }
        }

        private void OnStartGame()
        {
            if (!_isGameStarted)
            {
                ButtonStartGame();
            }
            else
            {
                ResumeGame();
            }
        }

        private void ContinueAfterDead()
        {
            PauseOrResume(false);
            _panelDie.gameObject.SetActive(!_panelDie.gameObject.activeSelf);
        }

        private void IsDeadPlayer()
        {
            _panelDie.gameObject.SetActive(true);
            PauseOrResume(true);
            OpenLinks.OpenURL(@"https://goldpromo.com/");
        }

        private void OnChangeExpirience(Expirience exp)
        {
            _panelHUD.Expirience.Update(exp.MaxValue, exp.CurrentValue);
            _panelHUD.TextPlayerLevel.text = _playerModel.PlayerStruct.Player.Expirience.CurrentLevel.ToString();
        }

        private void OnChangeShield(Shield shield)
        {
            _panelHUD.Shield.Update(shield.MaxValue, shield.CurrentValue);
        }

        private void OnChangeHealth(Health health)
        {
            _panelHUD.Health.Update(health.MaxHealth, health.CurrentHealth);
        }


        private void OnChangeTimeToLeftInGame(string time)
        {
            _panelResults.TextElapsedTime.text = time;
        }

        internal void Add(IEventAction eventAction)
        {
            _listEvent.Add(eventAction);

            if (eventAction is EnemyController enemyController)
            {
                enemyController.AddScoreByAsteroidDead += AddScoreByAsteroidDead;
            }

            if (eventAction is MoveController moveController && moveController is IEventActionGeneric<float> playerEvent)
            {
                playerEvent.OnChangePositionAxisY += PlayerEvent_EventFloatGeneric;
                moveController.OnChangeSpeedMovement += OnChangeSpeedMovement;
            }
        }

        private void AddScoreByAsteroidDead(float value)
        {
            _panelHUD.Score += value;
            _panelHUD.TextScore.text = _panelHUD.Score.ToString();
        }

        private void OnChangeSpeedMovement(float speed)
        {
            _panelResults.TextSpeed.text = speed.ToString("F0");
        }

        private void PlayerEvent_EventFloatGeneric(float value)
        {
            _distanceTravel += value;
            _panelResults.TextDistanceTraveled.text = _distanceTravel.ToString(format: "F0");

            _panelResults._sliderTravel.value = _distanceTravel / _playerModel.PlayerStruct.DistanceToMars;
        }

        public void Initialization()
        {
            _viewsStateActive = new Dictionary<PanelView, bool>();
         
            _panelMainMenu.ButtonQuit.onClick.AddListener(ApplicationQuit);

            UpdateHUDPlayer();
        }

        private void UpdateHUDPlayer()
        {
            OnChangeShield(_playerModel.PlayerStruct.Player.Shield);
            OnChangeHealth(_playerModel.PlayerStruct.Player.Health);
            OnChangeExpirience(_playerModel.PlayerStruct.Player.Expirience);
        }

        private void ButtonStartGame()
        {
            _panelMainMenu.gameObject.SetActive(true);
            _panelResults.gameObject.SetActive(true);
            _sceneControllerUIView.gameObject.SetActive(true);

            _isGameStarted = true;

            StartGame?.Invoke(EventCanvas.StartGame);

            if (_isPaused)
            {
                _isPaused = false;
                PauseOrResume(_isPaused);
            }

            _panelHUD.gameObject.SetActive(true);

            StartGame?.Invoke(EventCanvas.StartShip);

        }

        private void ResumeGame()
        {
            PauseOrResume(!_isPaused);
        }

        public void LateExecute(float deltatime)
        {
            //TODO - переделать - убрать в Сценеконтроллер, и подписываться на него...
            if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
            {
                ApplicationQuit();
            }
        }

        private void PauseOrResume(bool value)
        {
            _isPaused = value;
            SwitchPanelView(_isPaused);
            OnPause?.Invoke(_isPaused);
        }

        public void Execute(float deltatime)
        {
            _timerToLeftInGame.Execute(deltatime);
        }


        #region ICleanup

        public void Cleanup()
        {
            _playerModel.PlayerStruct.Player.Health.OnChangeHealth -= OnChangeHealth;
            _playerModel.PlayerStruct.Player.Shield.OnChangeShield -= OnChangeShield;
            _playerModel.PlayerStruct.Player.Expirience.OnChangeExpirience -= OnChangeExpirience;
            _playerModel.PlayerStruct.Player.IsDeadPlayer -= IsDeadPlayer;
            _panelDie.ButtonContinue.onClick.RemoveAllListeners();

            _sceneModel.SceneStruct.BroadcastEventManager.OnStartGame -= OnStartGame;

            foreach (var eventAction in _listEvent)
            {
                if (eventAction is EnemyController enemyController)
                {
                    enemyController.AddScoreByAsteroidDead -= AddScoreByAsteroidDead;
                }

                if (eventAction is MoveController moveController && moveController is IEventActionGeneric<float> playerEvent)
                {
                    playerEvent.OnChangePositionAxisY -= PlayerEvent_EventFloatGeneric;
                }
            }
            foreach (var button in _allButtonList)
            {
                button.onClick.RemoveAllListeners();
            }
            _allButtonList.Clear();
        }

        #endregion



        #region ApplicationQuit

        private void ApplicationQuit()
        {
            PauseOrResume(!_isPaused);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            OpenLinks.GoBackPage();
#else
            Application.Quit();
#endif
        }

        private void SwitchPanelView(bool isPaused)
        {
            foreach (var panelView in _canvasModel.CanvasStruct.CanvasView.panelViews)
            {
                if (panelView is panelDieView)
                    continue;
                if (isPaused)
                {
                    _viewsStateActive[panelView] = panelView.gameObject.activeSelf;
                    panelView.gameObject.SetActive(!isPaused);
                }
                else
                {
                    panelView.gameObject.SetActive(_viewsStateActive[panelView]);
                }
            }
        }

        #endregion
    }
}