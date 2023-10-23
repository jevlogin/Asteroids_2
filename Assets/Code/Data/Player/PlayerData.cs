using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/PlayerData", order = 51)]
    internal sealed class PlayerData : ScriptableObject
    {
        #region Fields

        [Header("Свойства игрока"), Space(20), SerializeField] private PlayerStruct _playerStruct;
        [Header("Компоненты игрока")] private PlayerComponents _playerComponents;
        [Header("Дополнительные настройки для игрока"), SerializeField] private PlayerSettings _playerSettings;

        #endregion


        #region Properties

        internal PlayerStruct PlayerStruct => _playerStruct;
        internal PlayerComponents PlayerComponents => _playerComponents;
        internal PlayerSettings PlayerSettings => _playerSettings;

        #endregion
    }
}