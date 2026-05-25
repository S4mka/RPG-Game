using UnityEngine;

namespace AdvancedRPG.Gameplay.Combat
{
    public readonly struct DamageData
    {
        public readonly float Amount;
        public readonly DamageType Type;
        public readonly GameObject Source;

        public DamageData(float amount, DamageType type, GameObject source)
        {
            Amount = amount;
            Type = type;
            Source = source;
        }
    }
}
