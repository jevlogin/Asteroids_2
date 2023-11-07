using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal struct PlayerStruct
    {
        #region Fields

        [SerializeField, Header("Сглаживание изменения скорости")] internal float VelocityChangeSpeed;
        [SerializeField, Header("Реальная скорость корабля")] internal float RealSpeedShipModel;
        [SerializeField, Header("Фактор множителя скорости и всего")] internal float ScaleFactor;
        internal float SpeedScale;
        internal Player Player;
        [SerializeField, Header("Скорость частиц после взлета")] internal float ParticleSpeedAfterTakeOff;

        #endregion
    }
}