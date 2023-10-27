namespace WORLDGAMEDEVELOPMENT
{
    internal interface IDamageable
    {
        float Damage {  get; set; }
        void TakeDamage(float damage);
    }
}