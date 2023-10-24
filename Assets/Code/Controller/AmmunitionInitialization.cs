namespace WORLDGAMEDEVELOPMENT
{
    internal class AmmunitionInitialization
    {
        private readonly AmmunitionFactory _ammunitionFactory;
        private readonly AmmunitionModel _ammunitionFactoryModel;

        public AmmunitionInitialization(AmmunitionFactory ammunitionFactory)
        {
            _ammunitionFactory = ammunitionFactory;
            _ammunitionFactoryModel = ammunitionFactory.CreateAmmunitionModel();
        }

        internal AmmunitionModel AmmunitionFactoryModel => _ammunitionFactoryModel;
    }
}