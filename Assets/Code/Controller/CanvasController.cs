using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasController : IController, ICleanup, IInitialization
    {
        private CanvasModel _canvasModel;

        private readonly PanelGameMenuView _panelGameMenu;
        private readonly PanelHUDView _panelHUD;
        private readonly PanelMainMenuView _panelMainMenu;

        private List<IEventAction> _listEvent = new();
        private List<Button> _allButtonList = new();

        public CanvasController(CanvasModel canvasModel)
        {
            _canvasModel = canvasModel;

            foreach (var panel in _canvasModel.CanvasStruct.CanvasView.panelViews)
            {
                if (panel is PanelGameMenuView panelMenu)
                {
                    _panelGameMenu = panelMenu;
                    _panelGameMenu.ButtonStart.onClick.AddListener(DisableMenu);
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
            _panelGameMenu.transform.gameObject.SetActive(false);
        }

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
            _panelMainMenu.ButtonQuit.onClick.AddListener(ApplicationQuit);
            _panelMainMenu.ButtonStart.onClick.AddListener(ButtonStartGame);
        }

        private void ButtonStartGame()
        {
            _panelMainMenu.gameObject.SetActive(false);
            _panelGameMenu.gameObject.SetActive(true);
        }

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
    }
}