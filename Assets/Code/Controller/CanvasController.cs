namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasController : IController, ICleanup
    {
        private CanvasModel _canvasModel;
        private PanelMenuView _panelMenu;

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
            }
        }

        private void DisableMenu()
        {
            _panelMenu.transform.gameObject.SetActive(false);
        }

        public void Cleanup()
        {
            _panelMenu.ButtonStart.onClick.RemoveAllListeners();
        }
    }
}