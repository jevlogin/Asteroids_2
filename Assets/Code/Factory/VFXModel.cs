namespace WORLDGAMEDEVELOPMENT
{
    internal class VFXModel
    {
        public VFXStruct VFXStruct;
        public VFXComponents Components;
        public VFXSettings Settings;

        public VFXModel(VFXStruct vfxStruct, VFXComponents components, VFXSettings settings)
        {
            VFXStruct = vfxStruct;
            Components = components;
            Settings = settings;
        }
    }
}