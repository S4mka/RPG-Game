using System;

namespace AdvancedRPG.Gameplay.Combat
{
    public interface IHealth
    {
        float Current { get; }
        float Max { get; }
        bool IsDead { get; }
        event Action<float, float> Changed;
        event Action Died;
        void Set(float current, float max);
        void Damage(float amount);
        void Heal(float amount);
    }
}
