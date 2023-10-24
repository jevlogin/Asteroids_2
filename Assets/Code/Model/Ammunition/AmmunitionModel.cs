namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class AmmunitionModel
    {
        #region Fields
        
        private AmmunitionStruct _amunitionStruct;
        private AmmunitionComponents _amunitionComponents;
        private AmmunitionSettings _amunitionSettings;

        #endregion


        #region Properties

        public AmmunitionStruct AmmunitionStruct => _amunitionStruct;
        public AmmunitionComponents AmmunitionComponents => _amunitionComponents;
        public AmmunitionSettings AmmunitionSettings => _amunitionSettings;

        #endregion


        #region ClassLifeCycles

        public AmmunitionModel(AmmunitionStruct ammunitionStruct, AmmunitionComponents components, AmmunitionSettings settings)
        {
            _amunitionStruct = ammunitionStruct;
            _amunitionComponents = components;
            _amunitionSettings = settings;
        } 

        #endregion
    }
}