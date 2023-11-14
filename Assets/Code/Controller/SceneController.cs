﻿using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class SceneController : IInitialization, ICleanup
    {
        private readonly SceneModel _sceneModel;
        private List<IAddedModel> _addedModels;

        internal event Action<bool> IsStopControl;
        internal event Action<bool> TakeOffOfTheShip;

        internal Action DisableEnergyBlock;
        private PanelGameMenuView _panelMenu;
        private PanelHUDView _panelHUD;
        private PlayerModel _playerModel;
        private PanelMainMenuView _panelMainMenu;

        internal IEnumerable<IAddedModel> AddedModels => _addedModels;

        public SceneController(SceneModel sceneModel)
        {
            _sceneModel = sceneModel;
            _addedModels = new();
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
                        _panelMainMenu.ButtonStart.onClick.AddListener(StartControl);
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
        }

        public void Cleanup()
        {
            _panelMenu.ButtonStart.onClick.RemoveAllListeners();
        }
    }
}