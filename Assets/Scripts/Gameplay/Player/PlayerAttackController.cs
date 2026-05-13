using AdvancedRPG.Gameplay.Combat;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AdvancedRPG.Gameplay.Player
{
    public sealed class PlayerAttackController : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField] private int physicalDamage = 25;
        [SerializeField] private float magicalDamage = 20f;
        [SerializeField] private float magicCooldown = 2f;

        [Header("References")]
        [SerializeField] private MeleeHitbox swordHitbox;
        [SerializeField] private MagicProjectile projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private LayerMask enemyMask;
        [SerializeField] private Animator animator;
        [SerializeField] private Image magicCooldownFill;

        [Header("New Input System Actions")]
        [SerializeField] private InputActionReference meleeAttackAction;
        [SerializeField] private InputActionReference magicAttackAction;

        private float nextMagicTime;

        private void OnEnable()
        {
            if (meleeAttackAction != null && meleeAttackAction.action != null)
            {
                meleeAttackAction.action.Enable();
                meleeAttackAction.action.performed += OnMeleeAttackPerformed;
            }

            if (magicAttackAction != null && magicAttackAction.action != null)
            {
                magicAttackAction.action.Enable();
                magicAttackAction.action.performed += OnMagicAttackPerformed;
            }
        }

        private void OnDisable()
        {
            if (meleeAttackAction != null && meleeAttackAction.action != null)
            {
                meleeAttackAction.action.performed -= OnMeleeAttackPerformed;
                meleeAttackAction.action.Disable();
            }

            if (magicAttackAction != null && magicAttackAction.action != null)
            {
                magicAttackAction.action.performed -= OnMagicAttackPerformed;
                magicAttackAction.action.Disable();
            }
        }

        private void Update()
        {
            if (magicCooldownFill != null)
                magicCooldownFill.fillAmount = Mathf.Clamp01((nextMagicTime - Time.time) / magicCooldown);
        }

        private void OnMeleeAttackPerformed(InputAction.CallbackContext context)
        {
            TryMeleeAttack();
        }

        private void OnMagicAttackPerformed(InputAction.CallbackContext context)
        {
            TryMagicAttack();
        }

        private void TryMeleeAttack()
        {
            animator?.SetTrigger("PAttack");
            swordHitbox?.Activate(gameObject, physicalDamage);
        }

        private void TryMagicAttack()
        {
            if (Time.time < nextMagicTime)
                return;

            if (projectilePrefab == null || projectileSpawnPoint == null)
                return;

            nextMagicTime = Time.time + magicCooldown;
            animator?.SetTrigger("MAttack");

            var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            
        }
    }
}
