using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossEnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Movement")]
    public float lookRadius = 20f;
    public float attackRange = 3f;

    [Header("Attack")]
    public int damage = 25;
    public int heavyDamage = 60;
    public float attackCooldown = 1.5f;
    public float heavyAttackCooldown = 6f;
    public float heavyAttackDuration = 1f;

    [Header("Health")]
    [SerializeField] private int maxHP = 800;
    public Slider healthbar;

    [Header("Mode")]
    public bool peacefulMode;

    [Header("Animation")]
    public Animator animator;

    private int HP;
    private bool wasHit;
    private float lastAttackTime;
    private float lastHeavyAttackTime;
    private float heavyAttackEndTime;
    private NavMeshAgent agent;
    private EnemyStateMachine stateMachine;

    private IdleState idleState;
    private AggressionState aggressionState;
    private AttackState attackState;
    private HeavyAttackState heavyAttackState;

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
        heavyAttackState = new HeavyAttackState(this);

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
        wasHit = true;

        if (HP <= 0)
        {
            Die();
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("hit");
        }

        if (target != null)
        {
            stateMachine.ChangeState(aggressionState);
        }
    }

    private bool HasTarget => target != null;
    private float DistanceToTarget => HasTarget ? Vector3.Distance(transform.position, target.position) : float.MaxValue;
    private bool CanUseHeavyAttack => Time.time >= lastHeavyAttackTime + heavyAttackCooldown;

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

    private void StopMoving()
    {
        agent.isStopped = true;
        SetRun(false);
    }

    private void RegularAttack()
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
        DealDamage(damage);
        Debug.Log("Boss attacked player");
    }

    private void StartHeavyAttack()
    {
        StopMoving();
        LookAtTarget();

        if (!CanUseHeavyAttack)
        {
            stateMachine.ChangeState(attackState);
            return;
        }

        lastHeavyAttackTime = Time.time;
        SetAttack(false);
        SetHeavyAttack(true);
        DealDamage(heavyDamage);
        Debug.Log("Boss used heavy attack");
    }

    private void DealDamage(int amount)
    {
        if (!HasTarget)
        {
            return;
        }

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(amount);
        }
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
        if (animator != null)
        {
            animator.SetBool("isRun", value);
        }
    }

    private void SetAttack(bool value)
    {
        if (animator != null)
        {
            animator.SetBool("isAttack", value);
        }
    }

    private void SetHeavyAttack(bool value)
    {
        if (animator != null)
        {
            animator.SetBool("isHeavyAttack", value);
        }
    }

    private void ResetAttackAnimations()
    {
        SetAttack(false);
        SetHeavyAttack(false);
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("death");
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

        Destroy(gameObject, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private class IdleState : IEnemyState
    {
        private readonly BossEnemyAI boss;

        public IdleState(BossEnemyAI boss)
        {
            this.boss = boss;
        }

        public void Enter()
        {
            boss.StopMoving();
            boss.ResetAttackAnimations();
        }

        public void Tick()
        {
            if (!boss.HasTarget)
            {
                return;
            }

            if (boss.peacefulMode && !boss.wasHit)
            {
                return;
            }

            if (boss.DistanceToTarget <= boss.lookRadius)
            {
                boss.stateMachine.ChangeState(boss.aggressionState);
            }
        }

        public void Exit()
        {
        }
    }

    private class AggressionState : IEnemyState
    {
        private readonly BossEnemyAI boss;

        public AggressionState(BossEnemyAI boss)
        {
            this.boss = boss;
        }

        public void Enter()
        {
            boss.ResetAttackAnimations();
        }

        public void Tick()
        {
            if (!boss.HasTarget)
            {
                boss.stateMachine.ChangeState(boss.idleState);
                return;
            }

            float distance = boss.DistanceToTarget;
            if (distance > boss.lookRadius && !boss.wasHit)
            {
                boss.stateMachine.ChangeState(boss.idleState);
                return;
            }

            if (distance <= boss.attackRange)
            {
                boss.stateMachine.ChangeState(boss.CanUseHeavyAttack ? boss.heavyAttackState : boss.attackState);
                return;
            }

            boss.MoveToTarget();
        }

        public void Exit()
        {
        }
    }

    private class AttackState : IEnemyState
    {
        private readonly BossEnemyAI boss;

        public AttackState(BossEnemyAI boss)
        {
            this.boss = boss;
        }

        public void Enter()
        {
            boss.StopMoving();
        }

        public void Tick()
        {
            if (!boss.HasTarget)
            {
                boss.stateMachine.ChangeState(boss.idleState);
                return;
            }

            if (boss.DistanceToTarget > boss.attackRange)
            {
                boss.stateMachine.ChangeState(boss.aggressionState);
                return;
            }

            if (boss.CanUseHeavyAttack)
            {
                boss.stateMachine.ChangeState(boss.heavyAttackState);
                return;
            }

            boss.RegularAttack();
        }

        public void Exit()
        {
            boss.SetAttack(false);
        }
    }

    private class HeavyAttackState : IEnemyState
    {
        private readonly BossEnemyAI boss;

        public HeavyAttackState(BossEnemyAI boss)
        {
            this.boss = boss;
        }

        public void Enter()
        {
            boss.StopMoving();
            boss.heavyAttackEndTime = Time.time + boss.heavyAttackDuration;
            boss.StartHeavyAttack();
        }

        public void Tick()
        {
            if (!boss.HasTarget)
            {
                boss.stateMachine.ChangeState(boss.idleState);
                return;
            }

            if (boss.DistanceToTarget > boss.attackRange)
            {
                boss.stateMachine.ChangeState(boss.aggressionState);
                return;
            }

            boss.LookAtTarget();

            if (Time.time >= boss.heavyAttackEndTime)
            {
                boss.stateMachine.ChangeState(boss.attackState);
            }
        }

        public void Exit()
        {
            boss.SetHeavyAttack(false);
        }
    }
}
