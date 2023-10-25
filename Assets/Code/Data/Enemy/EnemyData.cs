using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/EnemyData", order = 51)]
    internal sealed class EnemyData : ScriptableObject
    {
        [SerializeField] private EnemyStruct _enemyStruct;
        [SerializeField] private EnemyComponents _enemyComponents;
        [SerializeField] private EnemySettings _enemySettings;
    }
}