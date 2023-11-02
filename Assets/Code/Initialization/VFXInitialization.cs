namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class VFXInitialization
    {
        private readonly VFXModel _model;

        internal VFXModel Model => _model;

        public VFXInitialization(VFXFactory VFXFactory)
        {
            _model = VFXFactory.CreateVFXModel();
        }
    }
}