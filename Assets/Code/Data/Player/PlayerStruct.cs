using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct PlayerStruct
    {
        #region Fields

        [SerializeField, Header("Сглаживание изменения скорости")] internal float VelocityChangeSpeed;
        [SerializeField, Header("Разница квадрата скорости InputSystem")] internal float VelocityTrashHoldInput;

        [SerializeField, Header("Скорость перемещения гироскопа")] internal float VelocitySpeedGyro;
        [SerializeField, Header("Разница квадрата скорости гироскопа")] internal float VelocityTrashHoldGyro;
        [SerializeField, Header("Сглаживание перемещения гироскопа")] internal float VelocitySmooth;

        [SerializeField, Header("Реальная скорость корабля")] internal float RealSpeedShipModel;
        [SerializeField, Header("Фактор множителя скорости и всего")] internal float ScaleFactor;
        internal float SpeedScale;
        internal Player Player;
        [SerializeField, Header("Скорость частиц после взлета")] internal float ParticleSpeedAfterTakeOff;

        #endregion
    }
}