namespace WORLDGAMEDEVELOPMENT
{
    internal interface IMove
    {
        float Speed { get; }
        void Move(float horizontal, float vertical, float deltatime);
    }
}