using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasController : IController, ICleanup, IInitialization, ILateExecute, IExecute
    {
        private CanvasModel _canvasModel;
        private readonly PlayerModel _playerModel;
        private readonly PanelHUDView _panelHUD;
        private readonly PanelMainMenuView _panelMainMenu;
        private readonly PanelResultsView _panelResults;
        private List<IEventAction> _listEvent = new();
        private List<Button> _allButtonList = new();
        private bool _isPaused;
        private bool _isGameStarted;

        internal event Action<EventCanvas> StartGame;
        private Timer _timerToLeftInGame;
        private Timer _timerLevelLeft;
        private float _distanceTravel;
        private readonly SceneControllerUIView _sceneControllerUIView;

        public CanvasController(CanvasModel canvasModel, PlayerModel playerModel)
        {
            _canvasModel = canvasModel;
            _playerModel = playerModel;
            _playerModel.PlayerStruct.Player.Health.OnChangeHealth += OnChangeHealth;
            _playerModel.PlayerStruct.Player.Shield.OnChangeShield += OnChangeShield;

            _timerToLeftInGame = new Timer();
            _timerToLeftInGame.OnChangeTime += OnChangeTimeToLeftInGame;
            
            _timerLevelLeft = new Timer();

            foreach (var panel in _canvasModel.CanvasStruct.CanvasView.panelViews)
            {
                panel.gameObject.SetActive(false);

                if (panel is SceneControllerUIView sceneControllerUIView)
                {
                    _sceneControllerUIView = sceneControllerUIView;
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
        }

        public void Initialization()
        {
            if (!_panelMainMenu.gameObject.activeSelf)
                _panelMainMenu.gameObject.SetActive(true);

            _panelMainMenu.ButtonQuit.onClick.AddListener(ApplicationQuit);

            _panelMainMenu.ButtonStart.onClick.AddListener(ButtonStartGame);

            UpdateHUDPlayer();
        }

        private void UpdateHUDPlayer()
        {
            OnChangeShield(_playerModel.PlayerStruct.Player.Shield);
            OnChangeHealth(_playerModel.PlayerStruct.Player.Health);
        }

        private void ButtonStartGame()
        {
            _panelMainMenu.gameObject.SetActive(false);
            _panelResults.gameObject.SetActive(true);

            _sceneControllerUIView.gameObject.SetActive(true);
            _sceneControllerUIView._textCurrentScene.text = "Волна: 1"; //TODO - serialization scenecontroller данные брать отсюда
            //тут надо бы таймер запустить на 2 минуты..

            _isGameStarted = true;

            //Воспроизводит звук ракеты...
            StartGame?.Invoke(EventCanvas.StartGame);

            if (_isGameStarted)
            {
                var text = _panelMainMenu.ButtonStart.transform.GetComponentsInChildren<TextMeshProUGUI>().First();
                if (text != null)
                {
                    text.text = ManagerName.CONTINUE;
                    _panelMainMenu.ButtonStart.onClick.RemoveAllListeners();
                    _panelMainMenu.ButtonStart.onClick.AddListener(ResumeGame);
                }
            }

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
            _panelMainMenu.gameObject.SetActive(!_panelMainMenu.gameObject.activeSelf);
            PauseOrResume(_panelMainMenu.gameObject.activeSelf);
        }

        public void LateExecute(float deltatime)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _isGameStarted)
            {
                _panelMainMenu.gameObject.SetActive(!_panelMainMenu.gameObject.activeSelf);

                PauseOrResume(_panelMainMenu.gameObject.activeSelf);
            }
        }

        private void PauseOrResume(bool value)
        {
            _isPaused = value;

            if (_isPaused)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }

        public void Execute(float deltatime)
        {
            _timerToLeftInGame.Execute(deltatime);
        }


        #region ICleanup

        public void Cleanup()
        {
            _playerModel.PlayerStruct.Player.Health.OnChangeHealth -= OnChangeHealth;

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
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            Application.ExternalEval("history.back()");
#else
            Application.Quit();
#endif

        }

        #endregion
    }
}