using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal class ObjectType : IObjectType
    {
        [SerializeField] private GroupObjectType _groupOfType = GroupObjectType.TransformObject;
        [SerializeField] private ViewObjectType _viewOfType = ViewObjectType.AdditionalType;


        public GroupObjectType GroupObjectType => _groupOfType;
        public ViewObjectType ViewObjectType => _viewOfType;
    }
}