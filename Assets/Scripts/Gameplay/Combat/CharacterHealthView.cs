using System;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Combat
{
    public class CharacterHealthView : MonoBehaviour, IDamageable
    {
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Death = Animator.StringToHash("death");

        [Header("Health")]
        [SerializeField] private int maxHp = 100;
        [SerializeField] private bool destroyOnDeath = true;
        [SerializeField] private float destroyDelay = 2f;

        [Header("Animation")]
        [SerializeField] private Animator animator;

        private IHealth health;
        public IHealth Health
        {
            get
            {
                EnsureHealth();
                return health;
            }
        }

        public event Action<CharacterHealthView> Died;
        public event Action<CharacterHealthView, DamageData> Damaged;

        private bool isDead;

        private void Awake()
        {
            EnsureHealth();

            if (animator == null)
                animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            Health.Died += HandleDeath;
        }

        private void OnDisable()
        {
            Health.Died -= HandleDeath;
        }

        public void TakeDamage(DamageData damage)
        {
            if (isDead)
                return;

            Debug.Log($"{name} took {damage.Amount} {damage.Type} damage");

            Health.Damage(damage.Amount);
            Damaged?.Invoke(this, damage);

            if (!isDead && animator != null)
                animator.SetTrigger(Hit);
        }

        public void SetHp(float current, float max)
        {
            Health.Set(current, max);
        }

        private void EnsureHealth()
        {
            if (health == null)
                health = new HealthModel(maxHp);
        }

        private void HandleDeath()
        {
            if (isDead)
                return;

            isDead = true;

            Debug.Log($"{name} died");

            if (animator != null)
                animator.SetTrigger(Death);

            Died?.Invoke(this);

            if (destroyOnDeath)
                Destroy(gameObject, destroyDelay);
        }
    }
}
