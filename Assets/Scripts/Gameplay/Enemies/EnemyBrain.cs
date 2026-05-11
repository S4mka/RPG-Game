using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Gameplay.Enemies.States;
using UnityEngine;
using UnityEngine.AI;

namespace AdvancedRPG.Gameplay.Enemies
{
    [RequireComponent(typeof(NavMeshAgent), typeof(CharacterHealthView))]
    public sealed class EnemyBrain : MonoBehaviour
    {
        public Transform Player { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public CharacterHealthView Health { get; private set; }
        public Animator Animator;
        public float AggroDistance = 10f;
        public float AttackDistance = 2f;
        public float LowHpPercent = 0.25f;
        public bool PeacefulMode;
        public MeleeHitbox MeleeHitbox;
        public MagicProjectile ProjectilePrefab;
        public Transform ProjectilePoint;
        public LayerMask PlayerMask;
        public float Damage = 15f;
        public bool IsRanged;

        private EnemyStateMachine stateMachine;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Health = GetComponent<CharacterHealthView>();
            stateMachine = new EnemyStateMachine();
            Health.DiedView += OnDied;
        }

        public void Construct(Transform player)
        {
            Player = player;
            stateMachine.ChangeState(new IdleState(this, stateMachine));
        }

        private void Update() => stateMachine.Tick();

        public float DistanceToPlayer => Player == null ? float.MaxValue : Vector3.Distance(transform.position, Player.position);
        public bool HasLowHp => Health.Health.Current / Health.Health.Max <= LowHpPercent;
        public void Attack()
        {
            Animator?.SetTrigger("Attack");
            if (IsRanged && ProjectilePrefab != null && ProjectilePoint != null)
            {
                var p = Instantiate(ProjectilePrefab, ProjectilePoint.position, ProjectilePoint.rotation);
                p.Launch(gameObject, (Player.position + Vector3.up - ProjectilePoint.position), Damage, PlayerMask);
            }
            else MeleeHitbox?.Activate(gameObject, Damage);
        }
        private void OnDied(CharacterHealthView _) { FindObjectOfType<MobKillCounter>()?.AddKill(); }
    }
}
