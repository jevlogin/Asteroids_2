using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class VFXFactory
    {
        private VFXData _data;

        public VFXFactory(VFXData vFXData)
        {
            _data = vFXData;
        }

        internal VFXModel CreateVFXModel()
        {
            var vfxStruct = _data.Struct;
            var components = new VFXComponents();
            var settings = new VFXSettings();

            vfxStruct.PoolsVFX = new Dictionary<EnemyType, VFXPool>();

            foreach (var groupParticle in _data.Settings.ExplosionGroup)
            {
                var particle = Object.Instantiate(groupParticle.Particle);
                particle.name = groupParticle.Type.ToString();

                var poolParticle = new Pool<ParticleSystem>(particle, _data.Settings.PoolSize);

                components.TransformPoolParent ??= new GameObject(ManagerName.POOL_VFX).transform;

                var VFXPool = new VFXPool(poolParticle, components.TransformPoolParent);
                VFXPool.AddObjects(particle);

                vfxStruct.PoolsVFX[groupParticle.Type] = VFXPool;
            }

            var vfxModel = new VFXModel(vfxStruct, components, settings);
            return vfxModel;
        }
    }
}