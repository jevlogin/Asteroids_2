using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "AmmunitionData", menuName = "AmmunitionData/AmmunitionData", order = 51)]
    internal sealed class AmmunitionData : ScriptableObject
    {
        [SerializeField, Header("Свойства аммуниции")] private AmmunitionStruct _ammunitionStruct;
        [Header("Компоненты аммуниции")] private AmmunitionComponents _components;
        [SerializeField, Header("Дополнительные настройки аммуниции")] private AmmunitionSettings _settings;

        public AmmunitionStruct AmmunitionStruct => _ammunitionStruct;
        public AmmunitionComponents Components => _components;
        public AmmunitionSettings AmmunitionSettings => _settings;
    }
}