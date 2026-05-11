namespace AdvancedRPG.Gameplay.Combat
{
    public interface IDamageService
    {
        void Apply(IDamageable target, DamageData damage);
    }
}
