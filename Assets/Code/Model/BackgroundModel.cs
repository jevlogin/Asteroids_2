namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class BackgroundModel
    {
        public BackgroundStruct Structure;
        public BackgroundComponents Components;
        public BackgroundSettings Settings;

        public BackgroundModel(BackgroundStruct structure, BackgroundComponents components, BackgroundSettings settings)
        {
            Structure = structure;
            Components = components;
            Settings = settings;
        }
    }
}