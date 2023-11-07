using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "BackgroundData", menuName = "BackgroundData/BackgroundData", order = 51)]
    internal sealed class BackgroundData : ScriptableObject
    {
        [SerializeField] private BackgroundStruct sctructure;
        [SerializeField] private BackgroundComponents _components;
        [SerializeField] private BackgroundSettings _settings;

        internal BackgroundStruct Sctructure  => sctructure;
        internal BackgroundComponents Components  => _components;
        internal BackgroundSettings Settings  => _settings;
    }
}