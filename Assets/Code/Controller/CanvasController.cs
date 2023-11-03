using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasController : IController, ICleanup, IInitialization, ILateExecute
    {
        private CanvasModel _canvasModel;

        private readonly PanelGameMenuView _panelGameMenu;
        private readonly PanelHUDView _panelHUD;
        private readonly PanelMainMenuView _panelMainMenu;

        private List<IEventAction> _listEvent = new();
        private List<Button> _allButtonList = new();
        private bool _isPaused;
        private bool _isGameStarted;

        internal event Action<EventCanvas> StartGame;

        public CanvasController(CanvasModel canvasModel)
        {
            _canvasModel = canvasModel;

            foreach (var panel in _canvasModel.CanvasStruct.CanvasView.panelViews)
            {
                panel.gameObject.SetActive(false);

                if (panel is PanelGameMenuView panelMenu)
                {
                    _panelGameMenu = panelMenu;
                }
                if (panel is PanelHUDView panelHUD)
                {
                    _panelHUD = panelHUD;
                }
                if (panel is PanelMainMenuView panelMainMenu)
                {
                    _panelMainMenu = panelMainMenu;
                }
            }

            foreach (var item in _canvasModel.CanvasStruct.CanvasView.transform.GetComponentsInChildren<Button>())
            {
                _allButtonList.Add(item);
            }
        }

        private void DisableMenu()
        {
            _panelGameMenu.gameObject.SetActive(false);
            _panelHUD.gameObject.SetActive(true);

            StartGame?.Invoke(EventCanvas.StartShip);

            if (_isPaused)
            {
                _isPaused = false;
                PauseOrResume(_isPaused);
            }
        }

        internal void Add(IEventAction eventAction)
        {
            _listEvent.Add(eventAction);

            if (eventAction is EnemyController enemyController && enemyController is IEventActionGeneric<float> enemyEvent)
            {
                enemyEvent.AddScoreByAsteroidDead += EnemyController_AddScoreByAsteroidDead;
            }
        }

        private void EnemyController_AddScoreByAsteroidDead(float value)
        {
            _panelHUD.Score += value;
            _panelHUD.TextScore.text = $"{ManagerName.TEXT_SCORE} {_panelHUD.Score} {((int)_panelHUD.Score).GetStringRub()}";
        }


        public void Initialization()
        {
            if (!_panelMainMenu.gameObject.activeSelf)
                _panelMainMenu.gameObject.SetActive(true);

            _panelMainMenu.ButtonQuit.onClick.AddListener(ApplicationQuit);

            _panelMainMenu.ButtonStart.onClick.AddListener(ButtonStartGame);
            _panelGameMenu.ButtonStart.onClick.AddListener(DisableMenu);

        }

        private void ButtonStartGame()
        {
            _panelMainMenu.gameObject.SetActive(false);
            _panelGameMenu.gameObject.SetActive(true);

            _isGameStarted = true;

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


        #region ICleanup

        public void Cleanup()
        {
            _panelGameMenu.ButtonStart.onClick.RemoveAllListeners();

            foreach (var eventAction in _listEvent)
            {
                if (eventAction is IEventActionGeneric<float> enemyEvent)
                {
                    enemyEvent.AddScoreByAsteroidDead -= EnemyController_AddScoreByAsteroidDead;
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
            Application.OpenURL("about:blank");
#else
            Application.Quit();
#endif

        }


        #endregion
    }
}