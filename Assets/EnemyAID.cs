using UnityEngine;
using UnityEngine.AI;

public class EnemyAID : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;

    [Header("Detection")]
    public float lookRadius = 15f;

    [Header("Attack Distance")]
    public float attackRange = 8f;
    public float stoppingDistance = 6f;

    [Header("Attack")]
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Animation")]
    public Animator anim;

    private float nextAttackTime;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;

        agent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            if (distance > 10f)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                anim.SetBool("isRun", true);
                anim.SetBool("isAttack", false);
            }
            else if (distance >= 5f && distance <= 10f)
            {
                agent.isStopped = true;
                anim.SetBool("isRun", false);
                LookTarget();

                if (Time.time >= nextAttackTime)
                {
                    anim.SetTrigger("Shoot");
                    ThrowProjectile();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
            else if (distance < 5f)
            {
                Vector3 retreatDir = (transform.position - target.position).normalized;
                Vector3 retreatPoint = transform.position + retreatDir * 3f;

                agent.isStopped = false;
                agent.SetDestination(retreatPoint);
                anim.SetBool("isRun", true);
            }
        }
    }

    private void ChaseTarget()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);

        anim.SetBool("isRun", true);
        anim.SetBool("isAttack", false);
    }

    private void AttackTarget()
    {
        agent.isStopped = true;

        anim.SetBool("isRun", false);
        LookTarget();

        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Shoot");
            anim.SetBool("isAttack", true);

            ThrowProjectile();

            nextAttackTime = Time.time + attackCooldown;
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }

    private void IdleState()
    {
        agent.isStopped = true;
        anim.SetBool("isRun", false);
        anim.SetBool("isAttack", false);
    }

    private void ThrowProjectile()
    {
        if (projectilePrefab == null || firePoint == null || target == null)
            return;

        Vector3 direction = (target.position - firePoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        Instantiate(projectilePrefab, firePoint.position, rotation);
    }

    private void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude == 0f)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}