using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class GroupObject : IGroupObject
    {
        #region Fields
        
        [SerializeField] private Transform _transform;
        [SerializeField] private List<ParticleSystem> _particleSystems;
        [SerializeField] private ObjectType _type;

        #endregion


        #region Properties

        public Transform Transform
        {
            get
            {
                return _transform;
            }
            set
            {
                _transform = value;
            }
        }

        public GroupObjectType GroupObjectType => _type.GroupObjectType;
        public ViewObjectType ViewObjectType => _type.ViewObjectType;
        public List<ParticleSystem> ParticleSystems => _particleSystems; 

        #endregion
    }
}