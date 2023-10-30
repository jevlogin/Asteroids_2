using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


namespace WORLDGAMEDEVELOPMENT
{
    internal class CanvasFactory
    {
        private CanvasData _canvasData;

        public CanvasFactory(CanvasData canvasData)
        {
            _canvasData = canvasData;
            var eventSystem = Object.FindAnyObjectByType<EventSystem>();
            if (eventSystem == null )
            {
                eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }

        internal CanvasModel CreateCanvasModel()
        {
            var canvasStruct = _canvasData.CanvasStruct;
            var canvasSettings = _canvasData.CanvasSettings;

            canvasStruct.CanvasView = Object.Instantiate(_canvasData.CanvasSettings.CanvasView);
            canvasStruct.CanvasView.name = _canvasData.CanvasSettings.CanvasView.name;

            var canvasModel = new CanvasModel(canvasStruct, canvasSettings);

            return canvasModel;
        }
    }
}