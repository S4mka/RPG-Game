using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Gameplay.Enemies;
using AdvancedRPG.Gameplay.Enemies.States;
using UnityEngine;
using UnityEngine.AI;

namespace AdvancedRPG.Gameplay.Boss
{
    [RequireComponent(typeof(NavMeshAgent), typeof(CharacterHealthView))]
    public sealed class BossBrain : MonoBehaviour
    {
        public Transform Player;
        public NavMeshAgent Agent { get; private set; }
        public CharacterHealthView Health { get; private set; }
        public Animator Animator;
        public float AggroDistance = 15f;
        public float AttackDistance = 3f;
        public float Damage = 30f;
        public MeleeHitbox MeleeHitbox;
        public MagicProjectile StrongProjectilePrefab;
        public Transform ProjectilePoint;
        public LayerMask PlayerMask;
        public bool WasHit { get; private set; }
        private EnemyStateMachine machine;
        public float AttackDelay => Health.Health.Current / Health.Health.Max < 0.5f ? 0.7f : 1.4f;
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>(); Health = GetComponent<CharacterHealthView>(); machine = new EnemyStateMachine();
            Health.Damaged += _ => WasHit = true;
        }
        private void Start()
        {
            if (Player == null) Player = GameObject.FindGameObjectWithTag("Player")?.transform;
            machine.ChangeState(new BossIdleState(this, machine));
        }
        private void Update() => machine.Tick();
        public float DistanceToPlayer => Player == null ? float.MaxValue : Vector3.Distance(transform.position, Player.position);
        public void Attack() { Animator?.SetTrigger("Attack"); MeleeHitbox?.Activate(gameObject, Damage); }
        public void StrongAttack()
        {
            Animator?.SetTrigger("StrongAttack");
            if (StrongProjectilePrefab == null) return;
            var p = Instantiate(StrongProjectilePrefab, ProjectilePoint.position, ProjectilePoint.rotation);
            p.Launch(gameObject, (Player.position + Vector3.up - ProjectilePoint.position), Damage * 1.7f, PlayerMask);
        }
    }
}
