using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "VFXData", menuName = "VFXData/VFXData", order = 51)]
    internal sealed class VFXData : ScriptableObject
    {
        [SerializeField] private VFXStruct _struct;
        [SerializeField] private VFXSettings _settings;

        internal VFXStruct Struct  => _struct;
        internal VFXSettings Settings => _settings; 
    }
}