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
    public float fleeDistance = 12f;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 1.2f;

    [Header("Health")]
    [SerializeField] private int maxHP = 200;
    [SerializeField] private float fleeHealthPercent = 0.3f;
    public Slider healthbar;

    [Header("Mode")]
    public bool peacefulMode;

    [Header("Animation")]
    public Animator anim;

    private NavMeshAgent agent;
    private EnemyStateMachine stateMachine;
    private float lastAttackTime;
    private int HP;

    private IdleState idleState;
    private AggressionState aggressionState;
    private AttackState attackState;
    private FleeState fleeState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        HP = maxHP;

        if (target == null && PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            target = PlayerManager.instance.player.transform;
        }

        agent.stoppingDistance = attackRange;
        if (healthbar != null)
        {
            healthbar.maxValue = maxHP;
        }

        idleState = new IdleState(this);
        aggressionState = new AggressionState(this);
        attackState = new AttackState(this);
        fleeState = new FleeState(this);

        stateMachine = new EnemyStateMachine();
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        UpdateHealthbar();
        stateMachine.Tick();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            Die();
            return;
        }

        if (anim != null)
        {
            anim.SetTrigger("hit");
        }

        if (ShouldFlee)
        {
            stateMachine.ChangeState(fleeState);
        }
        else if (!peacefulMode && target != null)
        {
            stateMachine.ChangeState(aggressionState);
        }
    }

    private bool HasTarget => target != null;
    private bool ShouldFlee => HP <= maxHP * fleeHealthPercent;
    private float DistanceToTarget => HasTarget ? Vector3.Distance(transform.position, target.position) : float.MaxValue;

    private void MoveToTarget()
    {
        if (!HasTarget)
        {
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(target.position);
        SetRun(true);
    }

    private void FleeFromTarget()
    {
        if (!HasTarget)
        {
            StopMoving();
            return;
        }

        Vector3 fleeDirection = (transform.position - target.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * fleeDistance;

        agent.isStopped = false;
        agent.SetDestination(fleePosition);
        SetRun(true);
        SetAttack(false);
    }

    private void StopMoving()
    {
        agent.isStopped = true;
        SetRun(false);
    }

    private void TryAttack()
    {
        StopMoving();
        LookAtTarget();

        if (Time.time < lastAttackTime + attackCooldown)
        {
            SetAttack(false);
            return;
        }

        lastAttackTime = Time.time;
        SetAttack(true);

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        Debug.Log("Enemy attacked player");
    }

    private void LookAtTarget()
    {
        if (!HasTarget)
        {
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
        }
    }

    private void UpdateHealthbar()
    {
        if (healthbar != null)
        {
            healthbar.value = HP;
        }
    }

    private void SetRun(bool value)
    {
        if (anim != null)
        {
            anim.SetBool("isRun", value);
        }
    }

    private void SetAttack(bool value)
    {
        if (anim != null)
        {
            anim.SetBool("isAttack", value);
        }
    }

    private void Die()
    {
        if (anim != null)
        {
            anim.SetTrigger("death");
        }

        Collider enemyCollider = GetComponent<Collider>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (healthbar != null)
        {
            healthbar.gameObject.SetActive(false);
        }

        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, fleeDistance);
    }

    private class IdleState : IEnemyState
    {
        private readonly MeleeEnemyAI enemy;

        public IdleState(MeleeEnemyAI enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            enemy.StopMoving();
            enemy.SetAttack(false);
        }

        public void Tick()
        {
            if (!enemy.HasTarget)
            {
                return;
            }

            if (enemy.ShouldFlee)
            {
                if (enemy.DistanceToTarget < enemy.fleeDistance)
                {
                    enemy.stateMachine.ChangeState(enemy.fleeState);
                }

                return;
            }

            if (!enemy.peacefulMode && enemy.DistanceToTarget <= enemy.lookRadius)
            {
                enemy.stateMachine.ChangeState(enemy.aggressionState);
            }
        }

        public void Exit()
        {
        }
    }

    private class AggressionState : IEnemyState
    {
        private readonly MeleeEnemyAI enemy;

        public AggressionState(MeleeEnemyAI enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            enemy.SetAttack(false);
        }

        public void Tick()
        {
            if (!enemy.HasTarget)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
                return;
            }

            if (enemy.ShouldFlee)
            {
                enemy.stateMachine.ChangeState(enemy.fleeState);
                return;
            }

            float distance = enemy.DistanceToTarget;
            if (distance > enemy.lookRadius)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
                return;
            }

            if (distance <= enemy.attackRange)
            {
                enemy.stateMachine.ChangeState(enemy.attackState);
                return;
            }

            enemy.MoveToTarget();
        }

        public void Exit()
        {
        }
    }

    private class AttackState : IEnemyState
    {
        private readonly MeleeEnemyAI enemy;

        public AttackState(MeleeEnemyAI enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            enemy.StopMoving();
        }

        public void Tick()
        {
            if (!enemy.HasTarget)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
                return;
            }

            if (enemy.ShouldFlee)
            {
                enemy.stateMachine.ChangeState(enemy.fleeState);
                return;
            }

            float distance = enemy.DistanceToTarget;
            if (distance > enemy.attackRange)
            {
                enemy.stateMachine.ChangeState(enemy.aggressionState);
                return;
            }

            enemy.TryAttack();
        }

        public void Exit()
        {
            enemy.SetAttack(false);
        }
    }

    private class FleeState : IEnemyState
    {
        private readonly MeleeEnemyAI enemy;

        public FleeState(MeleeEnemyAI enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            enemy.SetAttack(false);
        }

        public void Tick()
        {
            if (!enemy.HasTarget)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
                return;
            }

            enemy.FleeFromTarget();

            if (enemy.DistanceToTarget >= enemy.fleeDistance)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
            }
        }

        public void Exit()
        {
            enemy.StopMoving();
        }
    }
}
