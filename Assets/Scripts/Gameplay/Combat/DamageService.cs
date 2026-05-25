namespace AdvancedRPG.Gameplay.Combat
{
    public sealed class DamageService : IDamageService
    {
        public void Apply(IDamageable target, DamageData damage)
        {
            target?.TakeDamage(damage);
        }
    }
}
