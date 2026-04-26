using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeleeEnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Movement")]
    public float lookRadius = 10f;
    public float attackRange = 2f;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 1.2f;

    [Header("Animation")]
    public Animator anim;

    private NavMeshAgent agent;
    private float lastAttackTime;

    private int HP = 200;
    public Slider healthbar;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null && PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            target = PlayerManager.instance.player.transform;
        }

        agent.stoppingDistance = attackRange;
    }

    private void Update()
    {
        healthbar.value = HP;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= lookRadius)
        {
            ChaseTarget(distance);
        }
        else
        {
            IdleState();
        }
    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if(HP <= 0)
        {
            anim.SetTrigger("death");
            GetComponent<Collider>().enabled = false;
            healthbar.gameObject.SetActive(false);
            Destroy(gameObject, 2f);

        }
        else
        {
            anim.SetTrigger("hit");
        }
    }

    void ChaseTarget(float distance)
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);

        if (distance > attackRange)
        {
            anim.SetBool("isRun", true);
            anim.SetBool("isAttack", false);
        }
        else
        {
            agent.isStopped = true;
            LookAtTarget();

            anim.SetBool("isRun", false);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
            else
            {
                anim.SetBool("isAttack", false);
            }
        }
    }

    void Attack()
    {
        anim.SetBool("isAttack", true);

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        Debug.Log("Enemy attacked player");
    }

    void IdleState()
    {
        agent.isStopped = true;
        anim.SetBool("isRun", false);
        anim.SetBool("isAttack", false);
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