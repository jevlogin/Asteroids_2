namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class CanvasModel : IAddedModel
    {
        private CanvasStruct _canvasStruct;
        private CanvasSettings _canvasSettings;

        internal CanvasStruct CanvasStruct => _canvasStruct;

        public CanvasModel(CanvasStruct canvasStruct, CanvasSettings canvasSettings)
        {
            _canvasStruct = canvasStruct;
            _canvasSettings = canvasSettings;
        }
    }
}