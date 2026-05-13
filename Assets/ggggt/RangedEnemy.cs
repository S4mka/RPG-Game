using UnityEngine;

public class RangedEnemy : EnemyBase
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float attackRange = 8f;
    public float attackCooldown = 3f;

    private float lastAttackTime;

    protected override void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position);

            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            base.Update();
        }
    }

    protected override void Attack()
    {
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        //// Должно быть:
       //projectile.GetComponent<Projectile>().Init(new DamageData(15f, DamageType.Magical));
    }
}