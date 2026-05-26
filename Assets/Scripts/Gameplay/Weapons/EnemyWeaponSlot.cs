using AdvancedRPG.Gameplay.Enemies;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Weapons
{
    [DisallowMultipleComponent]
    public sealed class EnemyWeaponSlot : MonoBehaviour
    {
        [SerializeField] private EnemyBrain enemyBrain;
        [SerializeField] private EnemyWeaponDefinition currentWeapon;
        [SerializeField] private bool applyOnStart = true;

        public EnemyWeaponDefinition CurrentWeapon => currentWeapon;

        private void Awake()
        {
            if (enemyBrain == null)
                enemyBrain = GetComponent<EnemyBrain>();
        }

        private void Start()
        {
            if (applyOnStart)
                ApplyCurrentWeapon();
        }

        public void SetWeapon(EnemyWeaponDefinition weapon)
        {
            currentWeapon = weapon;
            ApplyCurrentWeapon();
        }

        public void ApplyCurrentWeapon()
        {
            if (enemyBrain == null || currentWeapon == null)
                return;

            enemyBrain.ApplyWeapon(currentWeapon);
        }
    }
}
