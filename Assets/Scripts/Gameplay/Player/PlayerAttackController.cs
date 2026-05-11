using AdvancedRPG.Gameplay.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedRPG.Gameplay.Player
{
    public sealed class PlayerAttackController : MonoBehaviour
    {
        [SerializeField] private float physicalDamage = 25f;
        [SerializeField] private float magicalDamage = 20f;
        [SerializeField] private float magicCooldown = 2f;
        [SerializeField] private MeleeHitbox swordHitbox;
        [SerializeField] private MagicProjectile projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private LayerMask enemyMask;
        [SerializeField] private Animator animator;
        [SerializeField] private Image magicCooldownFill;

        private float nextMagicTime;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator?.SetTrigger("MeleeAttack");
                swordHitbox.Activate(gameObject, physicalDamage);
            }

            if (Input.GetMouseButtonDown(1) && Time.time >= nextMagicTime)
            {
                nextMagicTime = Time.time + magicCooldown;
                animator?.SetTrigger("MagicAttack");
                var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                projectile.Launch(gameObject, projectileSpawnPoint.forward, magicalDamage, enemyMask);
            }

            if (magicCooldownFill != null)
                magicCooldownFill.fillAmount = Mathf.Clamp01((nextMagicTime - Time.time) / magicCooldown);
        }
    }
}
