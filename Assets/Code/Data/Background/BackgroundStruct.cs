using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct BackgroundStruct
    {
        internal Dictionary<SpaceLayerType, List<IBackgroundPool>> PoolsType;
    }
}