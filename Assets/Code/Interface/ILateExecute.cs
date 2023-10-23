namespace WORLDGAMEDEVELOPMENT
{
    internal interface ILateExecute : IController
    {
        void LateExecute(float deltatime);
    }
}