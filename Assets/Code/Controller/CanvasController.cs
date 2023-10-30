namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasController : IController, ICleanup
    {
        private CanvasModel _canvasModel;

        public CanvasController(CanvasModel canvasModel)
        {
            _canvasModel = canvasModel;
            _canvasModel.CanvasStruct.CanvasView.ButtonStart.onClick.AddListener(DisableMenu);
        }

        private void DisableMenu()
        {
            _canvasModel.CanvasStruct.CanvasView.PanelMenuStart.gameObject.SetActive(false);
        }

        public void Cleanup()
        {
            _canvasModel.CanvasStruct.CanvasView.ButtonStart.onClick.RemoveAllListeners();
        }
    }
}