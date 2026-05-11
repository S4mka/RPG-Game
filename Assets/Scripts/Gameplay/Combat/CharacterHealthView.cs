using System;
using AdvancedRPG.Gameplay.Enemies;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Combat
{
    public sealed class CharacterHealthView : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHp = 100f;
        [SerializeField] private Animator animator;
        [SerializeField] private bool destroyOnDeath;

        public IHealth Health { get; private set; }
        public bool IsPlayer { get; private set; }
        public event Action<CharacterHealthView> DiedView;
        public event Action<DamageData> Damaged;

        private void Awake()
        {
            Health = new HealthModel(maxHp);
            Health.Died += OnDied;
            IsPlayer = CompareTag("Player");
        }

        public void TakeDamage(DamageData damage)
        {
            if (Health.IsDead) return;
            Health.Damage(damage.Amount);
            Damaged?.Invoke(damage);
            if (!Health.IsDead) animator?.SetTrigger("Hit");
        }

        public void Restore(float current, float max) => Health.Set(current, max);

        private void OnDied()
        {
            animator?.SetTrigger("Death");
            DiedView?.Invoke(this);
            if (TryGetComponent<EnemyBrain>(out var enemy)) enemy.enabled = false;
            if (destroyOnDeath) Destroy(gameObject, 2f);
        }
    }
}
