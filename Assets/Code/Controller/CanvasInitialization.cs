namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasInitialization
    {
        private readonly CanvasFactory _canvasFactory;
        private CanvasModel _canvasModel;

        internal CanvasModel CanvasModel => _canvasModel;

        public CanvasInitialization(CanvasFactory canvasFactory)
        {
            _canvasFactory = canvasFactory;
            _canvasModel = _canvasFactory.CreateCanvasModel();
        }
    }
}