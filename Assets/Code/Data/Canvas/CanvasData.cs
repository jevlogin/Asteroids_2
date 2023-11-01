using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "CanvasData", menuName = "CanvasData/CanvasData", order = 51)]
    internal sealed class CanvasData : ScriptableObject
    {
        [SerializeField] private CanvasStruct _canvasStruct;
        [SerializeField] private CanvasSettings _canvasSettings;

        internal CanvasStruct CanvasStruct => _canvasStruct;
        internal CanvasSettings CanvasSettings => _canvasSettings;
    }
}