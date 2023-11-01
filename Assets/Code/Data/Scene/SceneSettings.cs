using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal class SceneSettings
    {
        [SerializeField] private StartSceneView _startSceneView;

        internal StartSceneView StartSceneView
        {
            get
            {
                return _startSceneView;
            }
        }
    }
}