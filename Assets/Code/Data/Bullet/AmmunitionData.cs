using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "AmmunitionData", menuName = "AmmunitionData/AmmunitionData", order = 51)]
    internal sealed class AmmunitionData : ScriptableObject
    {
        [SerializeField, Header("�������� ���������")] private AmmunitionStruct _bulletStruct;
        [Header("���������� ���������")] private AmmunitionComponents _components;
        [SerializeField, Header("�������������� ��������� ���������")] private AmmunitionSettings _settings;

        public AmmunitionSettings Settings => _settings;
        public AmmunitionComponents Components => _components;
        public AmmunitionSettings BulletSettings => _settings;
    }
}