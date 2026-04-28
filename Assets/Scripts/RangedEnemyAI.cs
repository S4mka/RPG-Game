using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Movement")]
    public float lookRadius = 15f;
    public float attackRange = 8f;
    

    [Header("Attack")]
    public float attackCooldown = 2f;
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Animation")]
    public Animator animator;

    private int HP = 100;
    public Slider healtbar;

    private NavMeshAgent agent;
    private float lastAttackTime;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null && PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            target = PlayerManager.instance.player.transform;
        }
    }

    private void Update()
    {
        healtbar.value = HP;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= lookRadius)
        {
            HandleCombat(distance);
        }
        else
        {
            IdleState();
        }
    }

    void HandleCombat(float distance)
    {
        LookAtTarget();

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);

            animator.SetBool("isRun", true);
            animator.SetBool("isAttack", false);
        }
        else
        {
            agent.isStopped = true;

            animator.SetBool("isRun", false);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Shoot();
                lastAttackTime = Time.time;
            }
            else
            {
                animator.SetBool("isAttack", false);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            animator.SetTrigger("death");
            GetComponent<Collider>().enabled = false;
            healtbar.gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
        else 
        {
            animator.SetTrigger("hit");
        }
    }
    void Shoot()
    {
        animator.SetBool("isAttack", true);

        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(target);
            }
        }

        Debug.Log("Enemy shot projectile");
    }

    void IdleState()
    {
        agent.isStopped = true;
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack", false);
    }

    void LookAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}