using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal class SceneSettings
    {
        [SerializeField] private StartSceneView _startSceneView;
        [SerializeField] private BroadcastEventManager _broadcastEventManager;

        internal BroadcastEventManager BroadcastEventManager => _broadcastEventManager;
        internal StartSceneView StartSceneView => _startSceneView;
    }
}