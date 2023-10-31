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
            var canvasSettings = new CanvasSettings();

            canvasStruct.CanvasView = Object.Instantiate(_canvasData.CanvasSettings.CanvasView);
            canvasStruct.CanvasView.name = _canvasData.CanvasSettings.CanvasView.name;

            for (int i = 0; i < canvasStruct.CanvasView.transform.childCount; i++)
            {
                if (canvasStruct.CanvasView.transform.GetChild(i).TryGetComponent<PanelView>(out var panel))
                {
                    canvasStruct.CanvasView.panelViews.Add(panel);
                }
            }

            var canvasModel = new CanvasModel(canvasStruct, canvasSettings);

            return canvasModel;
        }
    }
}