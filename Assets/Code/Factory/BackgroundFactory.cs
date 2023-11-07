using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class BackgroundFactory
    {
        private readonly BackgroundData _data;

        public BackgroundFactory(BackgroundData backgroundData)
        {
            _data = backgroundData;
        }

        internal BackgroundModel CreateModel()
        {
            var structure = _data.Sctructure;
            var components = new BackgroundComponents();
            var settings = new BackgroundSettings(_data.Settings);

            components.TransformPoolParent ??= new GameObject(ManagerName.POOL_BACKGROUND).transform;

            Dictionary<SpaceLayerType, List<IBackgroundPool>> poolsType = new Dictionary<SpaceLayerType, List<IBackgroundPool>>();
            structure.PoolsType = poolsType;

            foreach (var backgroundObjects in settings.BackgroundObjectsByType.Values)
            {
                foreach (var background in backgroundObjects)
                {
                    var obj = Object.Instantiate(background.SpaceObject);
                    obj.name = background.SpaceLayerType.ToString();

                    switch (background.SpaceLayerType)
                    {
                        case SpaceLayerType.Background:
                            break;
                        case SpaceLayerType.Planets:
                            break;
                        case SpaceLayerType.SpaceStations:
                            break;
                        case SpaceLayerType.SatellitesAsteroids:
                            break;
                        case SpaceLayerType.SpecialEffects:
                            var particle = obj.GetOrAddComponent<ParticleSystem>();
                            var pool = new Pool<ParticleSystem>(particle, 1);

                            var poolFX = new VFXPool(pool, components.TransformPoolParent);
                            poolFX.AddObjects(particle);

                            if (!poolsType.ContainsKey(background.SpaceLayerType))
                            {
                                poolsType[background.SpaceLayerType] = new List<IBackgroundPool>();
                            }

                            poolsType[background.SpaceLayerType].Add(poolFX);

                            break;
                    }
                }
            }

            var backgroundModel = new BackgroundModel(structure, components, settings);
            return backgroundModel;
        }
    }
}