using System;
using AdvancedRPG.Gameplay.Combat;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Boss
{
    [Serializable]
    public sealed class BossLoadout
    {
        public string Name = "Boss Loadout";
        public BossWeaponType WeaponType = BossWeaponType.Melee;
        public BossElement Element = BossElement.Fire;
        public int Damage = 30;
        public float AttackDistance = 3f;
        public float AttackDelay = 1.4f;
        public MagicProjectile ProjectilePrefab;
        public AudioClip AttackSound;
        public AudioClip StrongAttackSound;
        public Color ElementColor = Color.white;
    }
}
