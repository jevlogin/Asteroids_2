using System;
using System.Collections.Generic;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasController : IController, ICleanup
    {
        private CanvasModel _canvasModel;
        private readonly PanelMenuView _panelMenu;
        private readonly PanelHUDView _panelHUD;
        private List<IEventAction> _listEvent = new();


        public CanvasController(CanvasModel canvasModel)
        {
            _canvasModel = canvasModel;

            foreach (var panel in _canvasModel.CanvasStruct.CanvasView.panelViews)
            {
                if (panel is PanelMenuView panelMenu)
                {
                    _panelMenu = panelMenu;
                    _panelMenu.ButtonStart.onClick.AddListener(DisableMenu);
                }
                if (panel is PanelHUDView panelHUD)
                {
                    _panelHUD = panelHUD;
                }
            }
        }

        private void DisableMenu()
        {
            _panelMenu.transform.gameObject.SetActive(false);
        }

        public void Cleanup()
        {
            _panelMenu.ButtonStart.onClick.RemoveAllListeners();
            foreach (var eventAction in _listEvent)
            {
                if (eventAction is IEventActionGeneric<float> enemyEvent)
                {
                    enemyEvent.AddScoreByAsteroidDead -= EnemyController_AddScoreByAsteroidDead;
                }
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
            _panelHUD.TextScore.text = $"{ManagerName.TEXT_SCORE} {_panelHUD.Score} {GetRublesForm((int)_panelHUD.Score)}";
        }

        public string GetRublesForm(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Не бывает таких рублей.");
            }

            if (amount % 10 == 1 && amount % 100 != 11)
            {
                return ManagerName.TEXT_SCORE_PREFIX_ONE;
            }
            else if ((amount % 10 >= 2 && amount % 10 <= 4) && (amount % 100 < 12 || amount % 100 > 14))
            {
                return ManagerName.TEXT_SCORE_PREFIX_TWO;
            }
            else
            {
                return ManagerName.TEXT_SCORE_PREFIX_FIVE;
            }
        }
    }
}