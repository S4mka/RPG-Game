using System;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Combat
{
    public sealed class HealthModel : IHealth
    {
        public float Current { get; private set; }
        public float Max { get; private set; }
        public bool IsDead => Current <= 0f;

        public event Action<float, float> Changed;
        public event Action Died;

        public HealthModel(float maxHp)
        {
            Max = Mathf.Max(1f, maxHp);
            Current = Max;
        }

        public void Set(float current, float max)
        {
            Max = Mathf.Max(1f, max);
            Current = Mathf.Clamp(current, 0f, Max);
            Changed?.Invoke(Current, Max);
            if (IsDead) Died?.Invoke();
        }

        public void Damage(float amount)
        {
            if (IsDead || amount <= 0f) return;
            Current = Mathf.Clamp(Current - amount, 0f, Max);
            Changed?.Invoke(Current, Max);
            if (IsDead) Died?.Invoke();
        }

        public void Heal(float amount)
        {
            if (IsDead || amount <= 0f) return;
            Current = Mathf.Clamp(Current + amount, 0f, Max);
            Changed?.Invoke(Current, Max);
        }
    }
}
