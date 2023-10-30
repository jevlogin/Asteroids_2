using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class CanvasSettings
    {
        [SerializeField] private CanvasView _canvasView;

        internal CanvasView CanvasView => _canvasView;
    }
}