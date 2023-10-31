using System;
using System.Collections.Generic;


namespace WORLDGAMEDEVELOPMENT
{
    internal class SceneController : IInitialization, ICleanup
    {
        private readonly SceneModel _sceneModel;
        private List<IAddedModel> _addedModels;

        internal event Action<bool> IsStopControl;
        internal event Action<bool> TakeOffOfTheShip;

        internal Action StartParticle;
        internal Action DisableEnergyBlock;
        private PanelMenuView _panelMenu;

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
                    if (panel is PanelMenuView panelMenu)
                    {
                        _panelMenu = panelMenu;
                        _panelMenu.ButtonStart.onClick.AddListener(StartControl);
                    }
                }
            }
        }

        private void TheShipTookOff(bool value)
        {
            IsStopControl?.Invoke(false);
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