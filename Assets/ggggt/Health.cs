using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private CharacterStats stats;

    public float CurrentHP { get; private set; }

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        CurrentHP = stats.maxHP;
    }

    public void TakeDamage(DamageData damage)
    {
        CurrentHP -= damage.Amount;

        OnHealthChanged?.Invoke(CurrentHP);

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject, 2f);
    }
}