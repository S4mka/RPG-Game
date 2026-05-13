using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    public float attackRange = 2f;
    public float damage = 10f;

    protected override void Update()
    {
        base.Update();

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
            Attack();
    }

    protected override void Attack()
    {
        IDamageable damageable = player.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(new DamageData(damage, DamageType.Physical));
    }
}