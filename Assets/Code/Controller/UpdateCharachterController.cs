using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WORLDGAMEDEVELOPMENT
{
    internal class UpdateCharachterController : ICleanup, IInitialization
    {
        #region Fields

        private readonly UpdateCharachtersView _updateCharView;
        private PlayerInitialization _playerInitialization;
        private CanvasInitialization _canvasInitialization;
        private List<Button> _listButtonsCleanUp;

        #endregion


        #region ClassLifeCycles

        public UpdateCharachterController(PlayerInitialization playerInitialization, CanvasInitialization canvasInitialization)
        {
            _playerInitialization = playerInitialization;
            _canvasInitialization = canvasInitialization;

            foreach (var panel in _canvasInitialization.CanvasModel.CanvasStruct.CanvasView.panelViews)
            {
                if (panel is UpdateCharachtersView updateCharView)
                {
                    _updateCharView = updateCharView;
                }
            }
        }

        #endregion


        #region IInitialization

        public void Initialization()
        {
            _listButtonsCleanUp = new List<Button>
            {
                _updateCharView.HealthPoints.ButtonConfirm,
                _updateCharView.Shield.ButtonConfirm,
                _updateCharView.Damage.ButtonConfirm,
                _updateCharView.Speed.ButtonConfirm,
            };

            _updateCharView.HealthPoints.ButtonConfirm.onClick.AddListener(UpdateHealth);

            _playerInitialization.PlayerModel.PlayerStruct.Player.Expirience.OnLevelUp += OnLevelUp;
            _playerInitialization.PlayerModel.PlayerStruct.Player.Expirience.OnChangeFreePoints += OnChangeFreePoints;
        }

        #endregion

        private void UpdateHealth()
        {
            if (_playerInitialization.PlayerModel.PlayerStruct.Player.Expirience.FreePoints > 0)
            {
                _playerInitialization.PlayerModel.PlayerStruct.Player.Health.MaxHealth += _playerInitialization.PlayerModel.PlayerStruct.Player.Health.ValuePercentUpdate;
                _playerInitialization.PlayerModel.PlayerStruct.Player.Expirience.FreePoints -= 1;

                //TODO - вынести в отдельный метод обновления вьюшки
                _updateCharView.HealthPoints.TextUpdateValue.text = _playerInitialization.PlayerModel.PlayerStruct.Player.Health.ValuePercentUpdate.ToString();
            }
        }

        private void OnChangeFreePoints(int freePoints)
        {
            if (freePoints <= 0)
            {
                _updateCharView.gameObject.SetActive(false);
            }

            _updateCharView.FreePoints.text = freePoints.ToString();
        }

        private void OnLevelUp(Expirience expirience)
        {
            if (!_updateCharView.gameObject.activeSelf)
            {
                _updateCharView.gameObject.SetActive(true); 
            }

            _updateCharView.HealthPoints.TextUpdateValue.text = _playerInitialization.PlayerModel.PlayerStruct.Player.Health.ValuePercentUpdate.ToString();
        }


        #region ICleanup

        public void Cleanup()
        {
            _playerInitialization.PlayerModel.PlayerStruct.Player.Expirience.OnLevelUp -= OnLevelUp;
            _playerInitialization.PlayerModel.PlayerStruct.Player.Expirience.OnChangeFreePoints -= OnChangeFreePoints;

            foreach (var button in _listButtonsCleanUp)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        #endregion
    }
}