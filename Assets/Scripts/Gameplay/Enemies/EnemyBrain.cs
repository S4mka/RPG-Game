using UnityEngine;
using UnityEngine.AI;
using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Gameplay.Enemies.States;
using AdvancedRPG.Gameplay.Weapons;

namespace AdvancedRPG.Gameplay.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterHealthView))]
    public class EnemyBrain : MonoBehaviour
    {
        private static readonly int IsRun = Animator.StringToHash("isRun");
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        private static readonly int Attack = Animator.StringToHash("attack");

        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private CharacterHealthView healthView;
        [SerializeField] private MeleeHitbox meleeHitbox;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private MagicProjectile projectilePrefab;

        [Header("Enemy Settings")]
        [SerializeField] private bool isRanged;
        [SerializeField] private bool peacefulMode;

        [Header("Distances")]
        [SerializeField] private float aggroDistance = 10f;
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private float rangedAttackDistance = 8f;
        [SerializeField] private float fleeHpPercent = 0.25f;

        [Header("Attack")]
        [SerializeField] private int damage = 10;
        [SerializeField] private DamageType damageType = DamageType.Physical;
        [SerializeField] private float attackCooldown = 1.5f;

        private EnemyStateMachine stateMachine;
        private float lastAttackTime;

        public Transform Target => target;
        public Animator Animator => animator;
        public NavMeshAgent Agent => agent;
        public CharacterHealthView HealthView => healthView;
        public MeleeHitbox MeleeHitbox => meleeHitbox;
        public MagicProjectile ProjectilePrefab => projectilePrefab;
        public Transform ProjectileSpawnPoint => projectileSpawnPoint;

        public bool IsRanged => isRanged;
        public bool PeacefulMode => peacefulMode;
        public int Damage => damage;
        public DamageType DamageType => damageType;
        public float AggroDistance => aggroDistance;
        public float AttackDistance => isRanged ? rangedAttackDistance : attackDistance;
        public float AttackCooldown => attackCooldown;

        private void Awake()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();

            if (healthView == null)
                healthView = GetComponent<CharacterHealthView>();

            if (animator == null)
                animator = GetComponent<Animator>();

            stateMachine = new EnemyStateMachine();
            FindTargetIfNeeded();
        }

        private void OnEnable()
        {
            if (healthView != null && healthView.Health != null)
                healthView.Health.Died += OnDied;
        }

        private void Start()
        {
            stateMachine.ChangeState(new IdleState(this));
        }

        private void Update()
        {
            FindTargetIfNeeded();
            stateMachine.Tick();
            UpdateMoveAnimation();
        }

        private void OnDisable()
        {
            if (healthView != null && healthView.Health != null)
                healthView.Health.Died -= OnDied;
        }

        public void Construct(Transform newTarget)
        {
            target = newTarget;
        }

        public void ApplyWeapon(EnemyWeaponDefinition weapon)
        {
            if (weapon == null)
                return;

            isRanged = weapon.IsRanged;
            damage = weapon.Damage;
            damageType = weapon.DamageType;
            attackCooldown = weapon.AttackCooldown;

            if (weapon.IsRanged)
            {
                rangedAttackDistance = weapon.AttackDistance;
                if (weapon.ProjectilePrefab != null)
                    projectilePrefab = weapon.ProjectilePrefab;
            }
            else
            {
                attackDistance = weapon.AttackDistance;
            }
        }

        public float DistanceToTarget()
        {
            if (target == null)
                return float.MaxValue;

            return Vector3.Distance(transform.position, target.position);
        }

        public bool CanAttack()
        {
            return Time.time >= lastAttackTime + attackCooldown;
        }

        public void MarkAttackTime()
        {
            lastAttackTime = Time.time;
        }

        public bool ShouldFlee()
        {
            if (healthView == null || healthView.Health == null)
                return false;

            float hpPercent = healthView.Health.Current / healthView.Health.Max;
            return hpPercent <= fleeHpPercent;
        }

        public void ChangeState(IEnemyState state)
        {
            stateMachine.ChangeState(state);
        }

        public void PlayAttackAnimation()
        {
            if (animator == null)
                return;

            animator.SetBool(IsAttack, true);
            animator.SetTrigger(Attack);
        }

        public void StopAttackAnimation()
        {
            if (animator == null)
                return;

            animator.SetBool(IsAttack, false);
        }

        private void FindTargetIfNeeded()
        {
            if (target != null)
                return;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        private void UpdateMoveAnimation()
        {
            if (animator == null || agent == null || !agent.enabled)
                return;

            bool isMoving = agent.velocity.sqrMagnitude > 0.05f;
            animator.SetBool(IsRun, isMoving);
        }

        private void OnDied()
        {
            if (agent != null)
            {
                agent.isStopped = true;
                agent.enabled = false;
            }

            if (animator != null)
            {
                animator.SetBool(IsRun, false);
                animator.SetBool(IsAttack, false);
            }
        }
    }
}
