using AdvancedRPG.Gameplay.Combat;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Weapons
{
    [CreateAssetMenu(menuName = "AdvancedRPG/Weapons/Enemy Weapon Definition", fileName = "EnemyWeaponDefinition")]
    public sealed class EnemyWeaponDefinition : ScriptableObject
    {
        [Header("Base")]
        [SerializeField] private string weaponName = "Enemy Weapon";
        [SerializeField] private EnemyWeaponType weaponType = EnemyWeaponType.Melee;
        [SerializeField] private DamageType damageType = DamageType.Physical;

        [Header("Stats")]
        [SerializeField] private int damage = 10;
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private float attackCooldown = 1.5f;

        [Header("Ranged Only")]
        [SerializeField] private MagicProjectile projectilePrefab;

        public string WeaponName => weaponName;
        public EnemyWeaponType WeaponType => weaponType;
        public DamageType DamageType => damageType;
        public int Damage => damage;
        public float AttackDistance => attackDistance;
        public float AttackCooldown => attackCooldown;
        public MagicProjectile ProjectilePrefab => projectilePrefab;
        public bool IsRanged => weaponType == EnemyWeaponType.Ranged;
    }
}
