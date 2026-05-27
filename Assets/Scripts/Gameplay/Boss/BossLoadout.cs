using System;
using UnityEngine;
using AdvancedRPG.Gameplay.Combat;

namespace AdvancedRPG.Gameplay.Boss
{
    [Serializable]
    public sealed class BossLoadout
    {
        [Header("Info")]
        public string Name;

        [Header("Weapon And Element")]
        public BossWeaponType WeaponType;
        public BossElement Element;

        [Header("Melee")]
        public GameObject MeleeWeaponObject;

        [Header("Ranged")]
        public MagicProjectile ProjectilePrefab;

        [Header("Stats")]
        public int Damage = 30;
        public float AttackDistance = 3f;
        public float AttackDelay = 1.5f;

        [Header("Visual")]
        public Color ElementColor = Color.white;

        [Header("Audio")]
        public AudioClip AttackSound;
        public AudioClip StrongAttackSound;
    }
}