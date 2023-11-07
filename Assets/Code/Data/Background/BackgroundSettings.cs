using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [System.Serializable ]
    internal sealed class BackgroundSettings
    {
        [SerializeField] private float backgroundScrollSpeed = 0.1f;
        [SerializeField] private float parallaxScrollSpeed = 0.5f; 

        [SerializeField] private List<SpaceBackgroundGroupObject> _backgroundObjects;
        private Dictionary<SpaceLayerType, List<SpaceBackgroundGroupObject>> _backgroundObjectsByType = new Dictionary<SpaceLayerType, List<SpaceBackgroundGroupObject>>();

        public BackgroundSettings(BackgroundSettings settings)
        {
            BackgroundScrollSpeed = settings.BackgroundScrollSpeed;
            ParallaxScrollSpeed = settings.ParallaxScrollSpeed;
            BackgroundObjects = new List<SpaceBackgroundGroupObject>();
            BackgroundObjects.AddRange(settings.BackgroundObjects);
            
            foreach (var backgroundObject in BackgroundObjects)
            {
                if (!BackgroundObjectsByType.ContainsKey(backgroundObject.SpaceLayerType))
                {
                    BackgroundObjectsByType[backgroundObject.SpaceLayerType] = new List<SpaceBackgroundGroupObject>();
                }
                BackgroundObjectsByType[backgroundObject.SpaceLayerType].Add(backgroundObject);
            }
        }

        public float BackgroundScrollSpeed { get => backgroundScrollSpeed; private set => backgroundScrollSpeed = value; }
        public float ParallaxScrollSpeed { get => parallaxScrollSpeed; private set => parallaxScrollSpeed = value; }
        internal List<SpaceBackgroundGroupObject> BackgroundObjects { get => _backgroundObjects; private set => _backgroundObjects = value; }
        internal Dictionary<SpaceLayerType, List<SpaceBackgroundGroupObject>> BackgroundObjectsByType { get => _backgroundObjectsByType; private set => _backgroundObjectsByType = value; }
    }
}